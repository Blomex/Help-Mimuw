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
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public UserActivityService(ILogger<UserActivityService> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task RegisterActionAsync(string name)
        {
            _logger.LogDebug($"Requested to register action datetime of user with name={name}");
            if (!_cache.ContainsKey(name) || DateTime.UtcNow.Subtract(_cache[name]) > TimeSpan.FromHours(1))
            {
                var user = await _repository.Users.FirstOrDefaultAsync(u => u.UserName == name);
                if (user != null)
                {
                    user.LastActive = DateTime.UtcNow;
                    await _repository.SaveChangesAsync();
                }
            }

            _cache.AddOrUpdate(name, DateTime.UtcNow, 
                (k, v) => DateTime.UtcNow > v ? DateTime.UtcNow : v);
        }

        public async Task<DateTime?> GetLastActionTimeAsync(string name)
        {
            _logger.LogDebug($"Requested last action datetime of user with name={name}");
            if (_cache.ContainsKey(name))
                return _cache[name];

            var user = await _repository.Users.FirstOrDefaultAsync(u => u.UserName == name);
            if (user != null)
            {
                _cache.AddOrUpdate(user.UserName, user.LastActive,
                    (k, v) => user.LastActive > v ? user.LastActive : v);
                return _cache[user.UserName];
            }

            return null;
        }
    }
}