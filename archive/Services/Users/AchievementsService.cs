using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace archive.Services.Users
{
    public class AchievementsService: IAchievementsService
    {
        private readonly ApplicationDbContext _database;

        public AchievementsService(ApplicationDbContext database)
        {
            _database = database;
        }

        public async Task<bool> DeclareAchievement(Achievement achievement)
        {
            if (await Find(achievement.NormalizedName) != null)
                return false;
            achievement.Id = 0;
            _database.Achievements.Add(achievement);
            await _database.SaveChangesAsync();
            return true;
        }

        public async Task GrantAchievement(ApplicationUser user, string achievementIdentifier)
        {
            var achievement = await Find(achievementIdentifier);

            if (await HasAchievement(user, achievement.Id))
                return;

            var entry = new UsersAchievements
            {
                UserId = user.Id,
                AchievementId = achievement.Id
            };
            _database.UsersAchievements.Add(entry);
            await _database.SaveChangesAsync();
        }

        public async Task<bool> HasAchievement(ApplicationUser user, string achievementNormalizedName)
        {
            return await HasAchievement(user, (await Find(achievementNormalizedName)).Id);
        }

        public async Task<ICollection<Achievement>> UsersAchievements(ApplicationUser user)
        {
            return await _database.UsersAchievements.Include(e => e.Achievement)
                .Where(e => e.UserId == user.Id)
                .Select(e => e.Achievement).ToListAsync();
        }

        protected async Task<bool> HasAchievement(ApplicationUser user, int achievementId)
        {
            return await _database.UsersAchievements
                .Where(e => e.UserId == user.Id && e.AchievementId == achievementId)
                .AnyAsync();
        }

        protected async Task<Achievement> Find(string identifier)
        {
            return await _database.Achievements.Where(e => e.NormalizedName == identifier)
                .FirstOrDefaultAsync();
        }
    }
}
