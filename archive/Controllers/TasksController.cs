using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using archive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace archive.Controllers
{
    public class TasksController : Controller
    {
        private readonly IRepository _repository;

        public TasksController(IRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        public async Task<ActionResult> Create()
        {
            var tasksets = (await _repository.Tasksets
                .Include(t => t.Course)
                .ToListAsync());
            return View(new CreateTaskViewModel(tasksets));
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTaskViewModel task)
        {
            _repository.Tasks
                .Add(new Data.Entities.Task
                {
                    TasksetId = task.TasksetId,
                    Name = task.Name,
                    Content = task.Content
                });
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index", "Home"); // TODO przekierować na zadania w zbiorze.
        }
    }
}