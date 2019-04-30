using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using archive.Data;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace archive.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly ConcurrentDictionary<string, DateTime> _cache = new ConcurrentDictionary<string, DateTime>();
        private readonly Timer _persistTimer = new Timer(30000); // in millis
        private readonly Timer _cleanCacheTimer = new Timer(600000); // in millis
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        private async void PersistCache(Object _, ElapsedEventArgs __)
        {
            foreach (var uid in _cache.Keys)
            {
                var user = await _repository.Users.FindAsync(uid);
                user.LastActive = _cache[uid];
            }

            await _repository.SaveChangesAsync();
        }

        public UserActivityService(ILogger logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _persistTimer.Elapsed += PersistCache;
            _persistTimer.AutoReset = true;
            _persistTimer.Enabled = true;
            _cleanCacheTimer.Elapsed += (_, __) => _cache.Clear();
            _cleanCacheTimer.AutoReset = true;
            _cleanCacheTimer.Enabled = true;
        }

        public void RegisterAction(string uid)
        {
            _cache.AddOrUpdate(uid, DateTime.Now, (_, __) => DateTime.Now);
        }

        public async Task<DateTime?> GetLastActionTimeAsync(string uid)
        {
            if (_cache.ContainsKey(uid))
                return _cache[uid];

            var user = await _repository.Users.FindAsync(uid);
            if (user != null)
            {
                _cache[user.Id] = user.LastActive;
            }

            return user?.LastActive;
        }
    }
}