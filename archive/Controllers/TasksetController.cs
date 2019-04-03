using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Models;
using archive.Models.Taskset;
using archive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            return View("ShowTaskset", model);
        }

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
            return View("Index", model);
        }

        [Authorize]
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

        [Authorize]
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