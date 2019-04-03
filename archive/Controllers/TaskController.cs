using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using archive.Models;
using archive.Models.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace archive.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public TaskController(IRepository repository, ILogger<TaskController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var tasksets = (await _repository.Tasksets
                .Include(t => t.Course)
                .ToListAsync());
            return View(new CreateTaskViewModel(tasksets));
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel task)
        {
            _logger.LogDebug($"Requested to add task: " + task);
            var tasksetForTask = await _repository.Tasksets
                .Where(t => t.Id == task.TasksetId)
                .FirstOrDefaultAsync();
                
            if (tasksetForTask == null || !ModelState.IsValid)
            {
                _logger.LogDebug($"Cannot add task: " + task);
                return new StatusCodeResult(400);
            }
            
            _repository.Tasks
                .Add(new Data.Entities.Task
                {
                    TasksetId = task.TasksetId,
                    Name = task.Name,
                    Content = task.Content
                });
            
            await _repository.SaveChangesAsync();
            return RedirectToAction("ShowTaskset", "Taskset", new { id = task.TasksetId });
        }
    }
}