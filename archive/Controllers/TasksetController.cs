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
using archive.Services;
using Microsoft.AspNetCore.Http;
using Task = System.Threading.Tasks.Task;

namespace archive.Controllers
{
    public class TasksetController : AbstractArchiveController
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;
        private readonly IStorageService _storageService;

        public TasksetController(ILogger<TasksetController> logger, IRepository repository,
            IUserActivityService activityService, IStorageService storageService) : base(activityService)
        {
            _logger = logger;
            _repository = repository;
            _storageService = storageService;
        }

        [Authorize]
        public async Task<IActionResult> ShowTaskset(int id)
        {
            var taskset = await _repository.Tasksets
                .Include(t => t.Course)
                .Include(t => t.Attachments)
                .ThenInclude(a => a.File)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (taskset == null)
            {
                return new StatusCodeResult(404);
            }

            if (taskset.Course.Archive == true || !ModelState.IsValid)
            {
                return new StatusCodeResult(403);
            }

            var tasks = await _repository.Tasks.Where(s => s.TasksetId == id).ToListAsync();

            var listOfSolutions = new Dictionary<int, List<Solution>>();

            foreach (var task in tasks)
            {
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

            if (course.Archive == true)
            {
                return new StatusCodeResult(403);
            }

            var tasksets = await _repository.Tasksets
                .Where(t => t.CourseId == id)
                .OrderByDescending(t => t.Year)
                .ToListAsync();

            var model = new IndexViewModel {Tasksets = tasksets, Course = course};
            // This is called also from HomeController.Shortcut and becouse of this we need full path to view file
            return View("/Views/Taskset/Index.cshtml", model);
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> RemoveAttachment(int tasksetId, string fileId)
        {
            _logger.LogDebug($"Requested to remove attachment={fileId} from taskset={tasksetId}");
            var taskset = await _repository.Tasksets
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == tasksetId);

            if (taskset == null)
            {
                _logger.LogDebug($"Cannot find taskset with id={tasksetId}");
                return new StatusCodeResult(404);
            }

            // "Detach" attachment, without removing file
            var toRemove = taskset.Attachments
                .FirstOrDefault(a => a.FileId.ToString() == fileId);
            taskset.Attachments.Remove(toRemove);
            await _repository.SaveChangesAsync();

            return await ShowTaskset(tasksetId);
        }

        private async Task StoreAttachments(Taskset entity, List<IFormFile> files)
        {
            // Store files attaching them to taskset
            foreach (var file in files)
            {
                var fileEntity = await _storageService.Store(file.FileName, file.OpenReadStream());
                entity.Attachments.Add(new TasksetsFiles() {TasksetId = entity.Id, FileId = fileEntity.Id});
            }
            
            await _repository.SaveChangesAsync();
        }

        
        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> AddAttachments(AddAttachmentsModel add)
        {
            _logger.LogDebug($"Requested to add attachments to taskset={add.TasksetId}");
            var taskset = await _repository.Tasksets
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == add.TasksetId);

            if (taskset == null)
            {
                _logger.LogDebug($"Cannot find taskset with id={add.TasksetId}");
                return new StatusCodeResult(404);
            }

            await StoreAttachments(taskset, add.Attachments);
            return await ShowTaskset(add.TasksetId);
        }

        [Authorize]
        public async Task<IActionResult> IndexFilter(int id, IndexViewModel model)
        {
            _logger.LogDebug($"Requested taskset for course id={id}");

            var course = await _repository.Courses.FindAsync(id);
            if (course == null)
            {
                _logger.LogDebug($"Cannot find course with id={id}");
                return new StatusCodeResult(404);
            }

            var tasksToShow = new List<archive.Data.Entities.Taskset>();

            if (model.haveSolutions)
            {
                var tasksets = await _repository.Solutions
                    .Where(e => e.Task.Taskset.CourseId == id
                                && e.Task.Taskset.Year >= model.yearFrom
                                && e.Task.Taskset.Year <= model.yearTo
                                && ((!model.haveTasks) ||
                                    e.Task.Taskset.Tasks
                                        .Any())) //Not sure if needed, solution shouldn't exist without task
                    .Select(s => s.Task.Taskset).Distinct()
                    .ToListAsync();
                tasksToShow.AddRange(tasksets.GetRange(0, tasksets.Count));
            }
            else
            {
                var tasksets = await _repository.Tasksets
                    .Where(s => s.CourseId == id
                                && s.Year >= model.yearFrom
                                && s.Year <= model.yearTo
                                && ((!model.haveTasks) || s.Tasks.Any()))
                    .OrderByDescending(s => s.Year)
                    .ToListAsync();
                tasksToShow.AddRange(tasksets.GetRange(0, tasksets.Count));
            }

            model.Tasksets = tasksToShow;
            model.Course = course;
            // This is called also from HomeController.Shortcut and becouse of this we need full path to view file
            return View("/Views/Taskset/Index.cshtml", model);
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> Create(int? forCourseId)
        {
            var course = await _repository.Courses
                .Where(c => c.Id == forCourseId
                            && c.Archive == false)
                .FirstOrDefaultAsync();

            if (course != null)
            {
                return View(new CreateTasksetViewModel(course));
            }

            //if course was not selected, user will be allowed to choose course
            var courses = await _repository
                .Courses.Where(e => e.Archive == false)
                .ToListAsync();
            return View(new CreateTasksetViewModel(courses));
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTasksetViewModel taskset)
        {
            // Validate request
            _logger.LogDebug($"Requested to create taskset:" + taskset);
            var same = await _repository.Tasksets
                .Where(t => t.Name == taskset.Name && t.Year == taskset.Year)
                .FirstOrDefaultAsync();

            if (same != null || !ModelState.IsValid)
            {
                _logger.LogDebug($"Cannot add:" + taskset);
                return new StatusCodeResult(400);
            }

            // Save taskset
            var entity = new Taskset
            {
                Type = taskset.TypeAsEnum,
                Name = taskset.Name,
                Year = taskset.Year,
                CourseId = taskset.CourseId,
            };

            _repository.Tasksets.Add(entity);
            await _repository.SaveChangesAsync();

            entity.Attachments = new List<TasksetsFiles>();
            if (taskset.Attachments != null)
            {
                await StoreAttachments(entity, taskset.Attachments);
            }

            return await Index(taskset.CourseId);
        }

        [Authorize(Roles = UserRoles.MODERATOR)]
        public async Task<IActionResult> Delete(int id)
        {
            var taskset = await _repository.Tasksets.FindAsync(id);
            if (taskset == null)
            {
                _logger.LogDebug($"Task(Id={id}) not found");
                return new StatusCodeResult(404);
            }

            // We can only delete empty tasksets
            int tasksCount = await _repository.Tasks.Where(t => t.TasksetId == taskset.Id).CountAsync();
            if (tasksCount > 0)
            {
                _logger.LogDebug($"Taskset(Id={id}) it nonempty and cannot be deleted");
                return new StatusCodeResult(400);
            }

            _repository.Tasksets.Remove(taskset);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index", new {id = taskset.CourseId});
        }

        public async Task<IActionResult> AddAttachmentsView(int tasksetId)
        {
            var taskset = await _repository.Tasksets
                .Include(t => t.Course)
                .Include(t => t.Attachments)
                .ThenInclude(a => a.File)
                .FirstOrDefaultAsync(t => t.Id == tasksetId);

            if (taskset == null)
            {
                _logger.LogDebug($"Cannot find taskset with id={tasksetId}");
            }

            return View("AddAttachments", new AddAttachmentsModel() {Taskset = taskset});
        }
    }
}