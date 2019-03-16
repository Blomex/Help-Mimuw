using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using Microsoft.AspNetCore.Mvc;
using archive.Models;
using Microsoft.EntityFrameworkCore;

namespace archive.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var courses = _repository.Courses
                .Select(c => new CourseViewModel(c.Name))
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(await courses);
        }

        public async Task<IActionResult> Tasksets(string courseName)
        {
            var tasksets = _repository.Tasksets
                .Where(t => t.Course.Name == courseName)
                .Select(t => new TasksetViewModel(t.Type, t.Year, t.Name))
                .OrderBy(t => t.Name)
                .ToListAsync();
            
            return View("Tasksets", new TasksetsViewModel(await tasksets, courseName));
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