using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using Microsoft.AspNetCore.Mvc;
using archive.Models;
using archive.Models.Course;
using archive.Models.Taskset;
using archive.Services;
using Microsoft.EntityFrameworkCore;

namespace archive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ITasksetService _tasksetService;
        private readonly ITaskService _taskService;
        private readonly ISolutionService _solutionService;

        public HomeController(ICourseService courseService, ITasksetService tasksetService, ITaskService taskService, ISolutionService solutionService)
        {
            _courseService = courseService;
            _tasksetService = tasksetService;
            _taskService = taskService;
            _solutionService = solutionService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _courseService.FindAllAsync())
                .Select(c => new CourseViewModel(c.Name, c.Id))
                .OrderBy(c => c.Name));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
