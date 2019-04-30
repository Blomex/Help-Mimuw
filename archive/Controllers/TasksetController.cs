using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Models.Taskset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using archive.Data.Enums;

namespace archive.Controllers
{
    public class TasksetController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public TasksetController(ILogger<TasksetController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [Authorize]
        public async Task<IActionResult> ShowTaskset(int id)
        {
            var taskset = await _repository.Tasksets
                .Include(t => t.Course)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (taskset == null)
            {
                return new StatusCodeResult(404);
            }

            var tasks = await _repository.Tasks.Where(s => s.TasksetId == id).ToListAsync();

            var listOfSolutions = new Dictionary<int, List<Solution>>();

            foreach (var task in tasks) {
                var solutions = await _repository.Solutions.Where(s => s.TaskId == task.Id).ToListAsync();
                listOfSolutions.Add(task.Id, solutions);
            }

            var model = new TasksetViewModel {Taskset = taskset, Tasks = tasks, ListOfSolutions = listOfSolutions};
            // We need full path (see Index(id))
            return View("/Views/Taskset/ShowTaskset.cshtml", model);
        }

        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            _logger.LogDebug($"Requested taskset for course id={id}");

            var course = await _repository.Courses.FindAsync(id);
            if (course == null)
            {
                _logger.LogDebug($"Cannot find course with id={id}");
                return new StatusCodeResult(404);
            }
            
            var tasksets = await _repository.Tasksets
                .Where(s => s.CourseId == id)
                .OrderByDescending(s => s.Year)
                .ToListAsync();

            var model = new IndexViewModel {Tasksets = tasksets, Course = course};
            // This is called also from HomeController.Shortcut and becouse of this we need full path to view file
            return View("/Views/Taskset/Index.cshtml", model);
        }

        [Authorize(Roles = UserRoles.MODERATOR)]
        public async Task<IActionResult> Create(int? forCourseId)
        {
            var course = await _repository.Courses
                .Where(c => c.Id == forCourseId)
                .FirstOrDefaultAsync();

            if (course != null)
            {
                return View(new CreateTasksetViewModel(course));
            }
            
            var courses = await _repository.Courses.ToListAsync();
            return View(new CreateTasksetViewModel(courses));
        }

        [Authorize(Roles = UserRoles.MODERATOR)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTasksetViewModel taskset)
        {
            _logger.LogDebug($"Requested to create taskset:" + taskset);
            var same = await _repository.Tasksets
                .Where(t => t.Name == taskset.Name && t.Year == taskset.Year)
                .FirstOrDefaultAsync();

            if (same != null || !ModelState.IsValid)
            {
                _logger.LogDebug($"Cannot add:" + taskset);
                return new StatusCodeResult(400);
            }
            
            _repository.Tasksets
                .Add(new Taskset
                {
                    Type = taskset.TypeAsEnum, 
                    Name = taskset.Name,
                    Year = taskset.Year,
                    CourseId = taskset.CourseId
                });
            
            await _repository.SaveChangesAsync();
            return await Index(taskset.CourseId);
        }
    }
}