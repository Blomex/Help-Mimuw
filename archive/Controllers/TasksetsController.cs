using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace archive.Controllers
{
    public class TasksetsController : Controller
    {
        private readonly IRepository _repository;

        public TasksetsController(IRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        public async Task<ActionResult> Create()
        {
            var courses = (await _repository.Courses.ToListAsync());
            return View(new CreateTasksetViewModel(courses));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(CreateTasksetViewModel taskset)
        {
            _repository.Tasksets
                .Add(new Taskset
                {
                    Type = taskset.TypeAsEnum, 
                    Name = taskset.Name,
                    Year = taskset.Year,
                    CourseId = taskset.CourseId
                });
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index", "Home"); // TODO przekierować na tasksety w przedmiocie.
        }
    }
}