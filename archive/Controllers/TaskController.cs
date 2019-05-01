using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Data.Enums;
using archive.Models.Task;
using archive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace archive.Controllers
{
    public class TaskController : ArchiveController
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(IRepository repository, ILogger<TaskController> logger, 
            IUserActivityService activityService, UserManager<ApplicationUser> userManager)
            : base(activityService)
        {
            _repository = repository;
            _logger = logger;
            _userManager = userManager;
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> Create(int? forTasksetId = null)
        {
            var taskset = await TasksetOrDefaultAsync(forTasksetId);
            if (taskset != null)
            {
                return View(new CreateTaskViewModel(taskset));
            }
            
            var tasksets = (await _repository.Tasksets
                .Include(t => t.Course)
                .ToListAsync());
            return View(new CreateTaskViewModel(tasksets));
        }
        
        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel task)
        {
            _logger.LogDebug($"Requested to add task: " + task);
            var tasksetForTask = await TasksetOrDefaultAsync(task.TasksetId);
                
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

        private Task<Taskset> TasksetOrDefaultAsync(int? id)
        {
            return _repository.Tasksets
                .Where(t => t.Id == id)
                .Include(t => t.Course)
                .FirstOrDefaultAsync();
        }

        [Authorize(Roles = UserRoles.MODERATOR)]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _repository.Tasks.Include(t => t.Taskset).Where(t => t.Id == id).FirstOrDefaultAsync();
            if (task == null)
            {
                _logger.LogDebug($"Task(Id={id}) not found");
                return new StatusCodeResult(404);
            }

            var model = new TaskEditModel
            {
                Id = task.Id,
                Taskset = task.Taskset,
                NewName = task.Name,
                NewContent = task.Content
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.MODERATOR)]
        public async Task<IActionResult> Edit([Bind] TaskEditModel edited)
        {
            _logger.LogDebug($"Requested to edit Task(Id={edited.Id})");
            // Retrieve task
            var task = await _repository.Tasks.Include(t => t.Taskset)
                .Where(t => t.Id == edited.Id).FirstOrDefaultAsync();
            if (task == null)
            {
                _logger.LogDebug($"Task(Id={edited.Id}) not found");
                return new StatusCodeResult(404);
            }

            // Validate
            if (!ModelState.IsValid)
            {
                _logger.LogDebug($"Edited task model was invalid");
                edited.Taskset = task.Taskset;
                return View(edited);
            }

            // Update and save
            task.Content = edited.NewContent;
            task.Name = edited.NewName;
            await _repository.SaveChangesAsync();

            return RedirectToAction("ShowTaskset", "Taskset", new { id = task.TasksetId });
        }
    }
}