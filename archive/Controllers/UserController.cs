using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using archive.Models.Taskset;
using archive.Models.User;
using archive.Services;
using archive.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace archive.Controllers
{
    public class UserController : AbstractArchiveController
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;
        private readonly IUserActivityService _activityService;
        private readonly IAchievementsService _achievementsService;

        public UserController(IRepository repository, ILogger<UserController> logger,
            IAchievementsService achievementsService,
            IUserActivityService activityService) : base(activityService)
        {
            _achievementsService = achievementsService;
            _repository = repository;
            _logger = logger;
            _activityService = activityService;
        }

        [Authorize]
        public async Task<IActionResult> ShowProfile(string name)
        {
            _logger.LogDebug($"Requested profile of user with name={name}");
            var user = await _repository.Users
                .Include(t => t.Avatar)
                .FirstOrDefaultAsync(t => t.UserName == name);

            if (user == null)
            {
                _logger.LogDebug($"Didn't find user with name={name}");
                return new StatusCodeResult(404);
            }

            var lastActive = await _activityService.GetLastActionTimeAsync(name);

            var achievements = await _achievementsService.UsersAchievements(user);
            return View("/Views/User/ShowProfile.cshtml", new ProfileViewModel
            {
                UserName = user.UserName,
                AvatarImage = user.Avatar?.Image,
                Email = user.Email,
                HomePage = user.HomePage,
                Phone = user.PhoneNumber,
                LastActive = lastActive,
                UserAchievements = achievements
            });
        }
    }
}