using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using Microsoft.AspNetCore.Mvc;
using archive.Models;
using archive.Services;
using Microsoft.EntityFrameworkCore;

namespace archive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ITasksetService _tasksetService;
        private readonly ITaskService _taskService;

        public HomeController(ICourseService courseService, ITasksetService tasksetService, ITaskService taskService)
        {
            _courseService = courseService;
            _tasksetService = tasksetService;
            _taskService = taskService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _courseService.FindAllAsync())
                .Select(c => new CourseViewModel(c.Name))
                .OrderBy(c => c.Name));
        }

        public async Task<IActionResult> Tasksets(string courseName)
        {
            var tasksets = (await _tasksetService.FindForCourseAsync(courseName))
                .Select(t => new TasksetViewModel(t.Type, t.Year, t.Name))
                .OrderByDescending(t => t.Year)
                .ThenBy(t => t.Name);

            return View("Tasksets", new TasksetsViewModel(tasksets, courseName));
        }

        public async Task<IActionResult> Tasks(string courseName, string tasksetName, int year)
        {
            var tasks = (await _taskService.FindForTasksetAsync(courseName, tasksetName, year))
                .Select(t => new TaskViewModel(t.Name, t.Content))
                .OrderBy(t => t.Name);

            return View("Tasks", new TasksViewModel(tasks, tasksetName, courseName));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> Tasks(string courseName, string tasksetName)
        {
            var tasks = _repository.Tasks
                .Where(t => t.Taskset.Name == tasksetName && t.Taskset.Course.Name == courseName)
                .Select(t => new TaskViewModel(t.Name, t.Content))
                .OrderBy(t => t.Name)
                .ToListAsync();

            return View("Tasks", new TasksViewModel(await tasks, tasksetName, courseName));
        }
    }
}
