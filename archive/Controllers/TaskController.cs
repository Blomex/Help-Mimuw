using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Data.Enums;
using archive.Models.Task;
using archive.Models.Taskset;
using archive.Services;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task = archive.Data.Entities.Task;

namespace archive.Controllers
{
    public class TaskController : AbstractArchiveController
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStorageService _storageService;
        private readonly MarkdownPipeline _markdownPipeline;

        public TaskController(
            IRepository repository, 
            ILogger<TaskController> logger,
            IUserActivityService activityService, 
            UserManager<ApplicationUser> userManager,
            IStorageService storageService,
            MarkdownPipeline markdownPipeline)
            : base(activityService)
        {
            _repository = repository;
            _logger = logger;
            _userManager = userManager;
            _storageService = storageService;
            _markdownPipeline = markdownPipeline;
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> Create(int? forTasksetId = null)
        {
            var taskset = await TasksetOrDefaultAsync(forTasksetId);
            if (taskset != null)
            {
                if (taskset.Course.Archive)
                    return new StatusCodeResult(403);
                return View(new CreateTaskViewModel(taskset));
            }

            var tasksets = (await _repository.Tasksets
                .Include(t => t.Course)
                .Where(t => t.Course.Archive == false)
                .ToListAsync());
            return View(new CreateTaskViewModel(tasksets));
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        [RequestSizeLimit(TasksetController.AttachmentRequestLimit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTaskViewModel task)
        {
            _logger.LogDebug($"Requested to add task: " + task);
            var tasksetForTask = await TasksetOrDefaultAsync(task.TasksetId);

            if (tasksetForTask == null || !ModelState.IsValid)
            {
                _logger.LogDebug($"Cannot add task: " + task);
                return new StatusCodeResult(400);
            }

            if (tasksetForTask.Course.Archive)
                return new StatusCodeResult(403);

            var entity = new Data.Entities.Task
            {
                TasksetId = task.TasksetId,
                Name = task.Name,
                Content = task.Content,
                CachedContent = Markdown.ToHtml(task.Content, _markdownPipeline)
            };

            _repository.Tasks.Add(entity);
            await _repository.SaveChangesAsync();
            await StoreAttachments(entity, task.Attachments);
            return RedirectToAction("ShowTaskset", "Taskset", new {id = task.TasksetId});
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
            var task = await _repository.Tasks
                .Include(t => t.Taskset)
                .Include(t => t.Attachments)
                .ThenInclude(a => a.File)
                .Where(t => t.Id == id).FirstOrDefaultAsync();
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
                NewContent = task.Content,
                Attachments = task.Attachments.Select(a => a.File).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.MODERATOR)]
        [RequestSizeLimit(TasksetController.AttachmentRequestLimit)]
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
            task.CachedContent = Markdown.ToHtml(edited.NewContent, _markdownPipeline);
            task.Name = edited.NewName;
            await _repository.SaveChangesAsync();

            return RedirectToAction("ShowTaskset", "Taskset", new {id = task.TasksetId});
        }

        [Authorize(Roles = UserRoles.MODERATOR)]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _repository.Tasks.FindAsync(id);
            if (task == null)
            {
                _logger.LogDebug($"Task(Id={id}) not found");
                return new StatusCodeResult(404);
            }

            _repository.Tasks.Remove(task);
            await _repository.SaveChangesAsync();
            return RedirectToAction("ShowTaskset", "Taskset", new {id = task.TasksetId});
        }

        private async System.Threading.Tasks.Task StoreAttachments(Task entity, List<IFormFile> files)
        {
            if (files == null) return;

            // Store files attaching them to task
            foreach (var file in files)
            {
                var fileEntity = await _storageService.Store(file.FileName, file.OpenReadStream());
                entity.Attachments.Add(new TasksFiles() {TaskId = entity.Id, FileId = fileEntity.Id});
            }

            await _repository.SaveChangesAsync();
        }

        /// FIXME: Metody Add/Remove Attachments to trochę copy pasta z TaksetsController
        /// Ale z drugiej strony nie ma tu wielkiej logiki i częściowo się różnią.
        /// Jednakże warto byłoby wygeneralizować metody niezależnie od typu.
        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> AddAttachmentsView(int taskId)
        {
            var task = await _repository.Tasks
                .Include(t => t.Taskset)
                .ThenInclude(t => t.Course)
                .Include(t => t.Attachments)
                .ThenInclude(a => a.File)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                _logger.LogDebug($"Cannot find task with id={taskId}");
                return new StatusCodeResult(404);
            }

            return View("AddAttachments", new AddAttachmentsModel() {Task = task, EntityId = taskId});
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> RemoveAttachment(int taskId, string fileId)
        {
            _logger.LogDebug($"Requested to remove attachment={fileId} from task={taskId}");
            var task = await _repository.Tasks
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                _logger.LogDebug($"Cannot find task with id={taskId}");
                return new StatusCodeResult(404);
            }

            var toRemove = task.Attachments
                .FirstOrDefault(a => a.FileId.ToString() == fileId);

            if (toRemove != null)
            {
                // Detach attachment, remove file — TODO może wystarczy tylko usunąć plik?
                task.Attachments.Remove(toRemove);
                await _storageService.Delete(toRemove.FileId);
            }
            else
            {
                _logger.LogDebug($"Cannot find file with id={fileId}");
                return new StatusCodeResult(404);
            }

            return await AddAttachmentsView(taskId);
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        [RequestSizeLimit(TasksetController.AttachmentRequestLimit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAttachments(AddAttachmentsModel add)
        {
            _logger.LogDebug($"Requested to add attachments to task={add.EntityId}");
            var task = await _repository.Tasks
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == add.EntityId);

            if (task == null)
            {
                _logger.LogDebug($"Cannot find task with id={add.EntityId}");
                return new StatusCodeResult(404);
            }

            await StoreAttachments(task, add.Attachments);
            return await AddAttachmentsView(add.EntityId);
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> EditTags(int taskid)
        {
            _logger.LogDebug($"Requested to edit tags to task id = {taskid} ");
            var task = await _repository.Tasks.Where(t => t.Id == taskid)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync();

            StringBuilder allTags = new StringBuilder("");
            foreach (var tag in task.Tags)
            {
                allTags.Append($"{tag.Name}, ");
            }

            return View(new EditTagsViewModel(task, allTags.ToString()));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> EditTags([Bind] EditTagsViewModel model)
        {
            var task = await _repository.Tasks.Where(t => t.Id == model.TaskId)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync();
            var tags = model.Tags?.Split(", ").ToList<string>();

            task.Tags.Clear();
            foreach (var newTag in tags)
            {
                task.Tags.Add(new Tag
                {
                    TaskId = model.TaskId,
                    Name = newTag
                });
            }

            await _repository.SaveChangesAsync();
            return RedirectToAction("ShowTaskset", "Taskset", new {id = task.TasksetId});
        }
    }
}