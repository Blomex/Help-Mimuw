using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using Microsoft.AspNetCore.Mvc;
using archive.Models;
using archive.Models.Taskset;
using archive.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using archive.Data.Entities;
using archive.Data.Enums;

namespace archive.Controllers
{
    public class HomeController : Controller
    {
        private readonly TasksetController _tasksetController;
        private readonly ILogger _logger;
        private readonly IRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<SolutionController> logger, IRepository repository, TasksetController tasksetController,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _repository = repository;
            _tasksetController = tasksetController;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _repository.Courses.OrderBy(c => c.Name).ToListAsync();
            return View(courses);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> Shortcut(string shcCourse, string shcTaskset=null, short? shcTask=null)
        {
            if (string.IsNullOrEmpty(shcCourse))
            {
                return new StatusCodeResult(400);
            }
            _logger.LogDebug($"Shortcut for course={shcCourse}, taskset={shcTaskset}, task={shcTask}");

            // Find course by shortcut
            var course = await _repository.Courses.Where(e => e.ShortcutCode == shcCourse).FirstAsync();
            if (course == null)
            {
                return new StatusCodeResult(404);
            }

            if (string.IsNullOrEmpty(shcTaskset))
            {
                return await _tasksetController.Index(course.Id);
            }

            // Find the exam by shortcut
            var taskset = await _repository.Tasksets
                .Where(e => e.CourseId == course.Id && e.ShortcutCode == shcTaskset).FirstAsync();
            if (taskset == null)
            {
                return new StatusCodeResult(404);
            }

            if (shcTask == null)
            {
                return await _tasksetController.ShowTaskset(taskset.Id);
            }

            return new StatusCodeResult(404);
        }

        [Authorize]
        public async Task<IActionResult> BloodyAuthorization()
        {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
            await _userManager.AddToRoleAsync(user, UserRoles.ARCHUSER);
            return new StatusCodeResult(200);
        }


        [Authorize(Roles = UserRoles.ARCHUSER)]
        public IActionResult TestBloodyAuthorization()
        {
            return new StatusCodeResult(200);
        }
    }
}
