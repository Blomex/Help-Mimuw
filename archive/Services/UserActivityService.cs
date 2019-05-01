using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Timers;
using archive.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace archive.Services
{
    public class UserActivityService : IUserActivityService
    {
        private static ConcurrentDictionary<string, DateTime> _cache = new ConcurrentDictionary<string, DateTime>();
        private static ConcurrentDictionary<string, bool> _cacheDirty = new ConcurrentDictionary<string, bool>();
        private static Timer _persistTimer = new Timer(30000); // in millis
        private static Timer _cleanCacheTimer = new Timer(600000); // in millis
        private static bool _persistCacheTimerElapsed = false;
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        private async Task PersistCache()
        {
            foreach (var name in _cache.Keys)
                if (_cacheDirty[name])
                {
                    var user = await _repository.Users.FirstOrDefaultAsync(u => u.UserName == name);
                    user.LastActive = _cache[name];
                    await _repository.SaveChangesAsync();
                    _cacheDirty[name] = false;
                }
        }

        static UserActivityService()
        {
            _persistTimer.Elapsed += (_, __) => _persistCacheTimerElapsed = true;
            _persistTimer.AutoReset = true;
            _persistTimer.Enabled = true;
            _cleanCacheTimer.Elapsed += (_, __) => _cache.Clear();
            _cleanCacheTimer.Elapsed += (_, __) => _cacheDirty.Clear();
            _cleanCacheTimer.AutoReset = true;
            _cleanCacheTimer.Enabled = true;
        }

        public UserActivityService(ILogger<UserActivityService> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task RegisterActionAsync(string name)
        {
            _logger.LogDebug($"Rejestracja akcji użytkownika: name={name}");
            _cache.AddOrUpdate(name, DateTime.UtcNow, (_, __) => DateTime.UtcNow);
            _cacheDirty.AddOrUpdate(name, true, (_, __) => true);

            if (_persistCacheTimerElapsed)
            {
                await PersistCache();
                _persistCacheTimerElapsed = false;
            }
        }

        public async Task<DateTime?> GetLastActionTimeAsync(string name)
        {
            _logger.LogDebug($"Zażadanie pobrania daty ostatniej akcji użytkownika: name={name}");
            if (_cache.ContainsKey(name))
                return _cache[name];

            var user = await _repository.Users.FirstOrDefaultAsync(u => u.UserName == name);
            if (user != null)
            {
                _cache.AddOrUpdate(user.UserName, user.LastActive,(_, __) => user.LastActive);
                return _cache[user.UserName];
            }
            return null;
        }
    }
}