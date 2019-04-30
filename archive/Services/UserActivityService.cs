using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using archive.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace archive.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly ConcurrentDictionary<string, DateTime> _cache = new ConcurrentDictionary<string, DateTime>();
        private readonly ConcurrentDictionary<string, bool> _cacheDirty = new ConcurrentDictionary<string, bool>();
        private readonly Timer _persistTimer = new Timer(30000); // in millis
        private readonly Timer _cleanCacheTimer = new Timer(600000); // in millis
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        private async void PersistCache(Object _, ElapsedEventArgs __)
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

        public UserActivityService(ILogger<UserActivityService> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _persistTimer.Elapsed += PersistCache;
            _persistTimer.AutoReset = true;
            _persistTimer.Enabled = true;
            _cleanCacheTimer.Elapsed += (_, __) => _cache.Clear();
            _cleanCacheTimer.Elapsed += (_, __) => _cacheDirty.Clear();
            _cleanCacheTimer.AutoReset = true;
            _cleanCacheTimer.Enabled = true;
        }

        public void RegisterAction(string name)
        {
            _cacheDirty.AddOrUpdate(name, true, (_, __) => true);
            _cache.AddOrUpdate(name, DateTime.Now, (_, __) => DateTime.Now);
        }

        public async Task<DateTime?> GetLastActionTimeAsync(string name)
        {
            if (_cache.ContainsKey(name))
                return _cache[name];

            var user = await _repository.Users.FirstOrDefaultAsync(u => u.UserName == name);
            if (user != null)
            {
                _cache.AddOrUpdate(user.Id, user.LastActive == new DateTime() ? user.LastActive : DateTime.Now,
                    (_, __) => user.LastActive == new DateTime() ? user.LastActive : DateTime.Now);
                _cacheDirty.AddOrUpdate(user.Id, true, (_, __) => true);
            }

            return user != null ? _cache[user.Id] : (DateTime?) null;
        }
    }
}