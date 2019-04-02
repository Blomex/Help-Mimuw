using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Models;
using archive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace archive.Controllers
{
    public class TasksetsController : Controller
    {
        private readonly ITasksetService _tasksetService;
        private readonly ICourseService _courseService;

        public TasksetsController(ITasksetService tasksetService, ICourseService courseService)
        {
            _tasksetService = tasksetService;
            _courseService = courseService;
        }

        [Authorize]
        public async Task<ActionResult> Create()
        {
            var courses = (await _courseService.FindAllAsync());
            return View(new CreateTasksetViewModel(courses));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(CreateTasksetViewModel taskset)
        {
            await _tasksetService
                .AddTasksetAsync(new Taskset
                {
                    Type = taskset.TypeAsEnum,
                    Name = taskset.Name,
                    Year = taskset.Year,
                    CourseId = taskset.CourseId
                });
            return RedirectToAction("Index", "Home"); // TODO przekierować na tasksety w przedmiocie.
        }
    }
}