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
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //check how deal with null
            var courses = await _repository.Courses.Where(c => c.Archive == false).OrderBy(c => c.Name).ToListAsync();
            return View(courses);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [Authorize(Roles = "MODERATOR")]
        public IActionResult CreateCourse()
        {

            return View(new CreateCourseModel{});
        }

        [HttpPost]
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> CreateCourse(CreateCourseModel model)
        {
            _logger.LogDebug($"Requested to add course: " + model.Name);
            
            
            _repository.Courses
                .Add(new Data.Entities.Course
                {
                    Name = model.Name,
                    ShortcutCode = model.ShortcutCode,
                    Archive = false
                
                });
            
            await _repository.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> EditCourse()
        {
            var courses = await _repository.Courses.ToListAsync();
            return View(new EditCourseModel(courses));
        }

        [HttpPost]
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> EditCourse(EditCourseModel model)
        {
            _logger.LogDebug($"Requested to edit name:" + model.Name);

            var course = await _repository.Courses
                .Where(c => c.Id == model.CourseId)
                .FirstOrDefaultAsync();

            if (course == null || !ModelState.IsValid)
            {
                _logger.LogDebug($"Cannot edit:" + model.Name);
                return new StatusCodeResult(400);
            }
            
            course.Name = model.Name;
            
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");;
        }

        [Authorize]
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> ArchiveCourse()
        {
            var courses = await _repository.Courses.Where(c => c.Archive == false).ToListAsync();
            return View(new ArchiveCourseModel(courses));
        } 

        [HttpPost]
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> ArchiveCourse(ArchiveCourseModel model)
        {
            _logger.LogDebug($"Archive to course id :" + model.CourseId);

            var course = await _repository.Courses
                .Where(c => c.Id == model.CourseId)
                .FirstOrDefaultAsync();

            if (course == null || !ModelState.IsValid)
            {
                _logger.LogDebug($"Cannot archive:" + model.CourseId);
                return new StatusCodeResult(400);
            }
            
            course.Archive = true;
            
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");;
        }

        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> UnarchiveCourse()
        {
            var courses = await _repository.Courses.Where(c => c.Archive == true).ToListAsync();
            return View(new ArchiveCourseModel(courses));
        } 

        [HttpPost]
        [Authorize(Roles = "MODERATOR")]
        public async Task<IActionResult> UnarchiveCourse(ArchiveCourseModel model)
        {
            _logger.LogDebug($"Archive to course id :" + model.CourseId);

            var course = await _repository.Courses
                .Where(c => c.Id == model.CourseId)
                .FirstOrDefaultAsync();

            if (course == null || !ModelState.IsValid)
            {
                _logger.LogDebug($"Cannot archive:" + model.CourseId);
                return new StatusCodeResult(400);
            }
            
            course.Archive = false;
            
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");;
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
    }
}
