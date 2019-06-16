using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Transactions;
using archive.Commons.Authorization;
using archive.Data;
using archive.Data.Entities;
using archive.Data.Enums;
using archive.Models;
using archive.Models.Comment;
using archive.Models.Solution;
using archive.Services;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;
using archive.Services.Users;
using Microsoft.CodeAnalysis;
using Solution = archive.Data.Entities.Solution;

namespace archive.Controllers
{
    public class SolutionController : AbstractArchiveController
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IStorageService _storageService;
        private readonly MarkdownPipeline _markdownPipeline;
        private readonly IAchievementsService _achievementsService;

        public SolutionController(
            ILogger<SolutionController> logger, 
            ApplicationDbContext repository, 
            UserManager<ApplicationUser> userManager, 
            IUserActivityService activityService,
            IAuthorizationService authorizationService, 
            IStorageService storageService,
            IAchievementsService achiemenetService,
            MarkdownPipeline markdownPipeline
            ) : base(activityService)
        {
            _logger = logger;
            _repository = repository;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _storageService = storageService;
            _achievementsService = achiemenetService;
            _markdownPipeline = markdownPipeline;
        }

        [Authorize]
        public async Task<IActionResult> Show(int solutionId)
        {
            _logger.LogDebug($"Requested solution with id={solutionId}");

            var solution = await _repository.Solutions
                .Include(s => s.Author)
                .Include(s => s.Attachments)
                .ThenInclude(a => a.File)
                .FirstOrDefaultAsync(s => s.Id == solutionId);
            if (solution == null)
            {
                _logger.LogDebug($"Solution with id={solutionId} not found");
                return new StatusCodeResult(404);
            }

            var task = await _repository.Tasks
                .Include(t => t.Taskset.Course)
                .FirstOrDefaultAsync(t => t.Id == solution.TaskId);
            if (task == null)
            {
                _logger.LogWarning($"Solution with id={solutionId} exists but references task with " +
                    $"id={solution.TaskId} which does not");
                return new StatusCodeResult(404); // Surely?
            }

            if (task.Taskset.Course.Archive)
            {
                _logger.LogWarning(($"Solution with id={solutionId} exists but course is archived"));
                return new StatusCodeResult(404);
            }

            _logger.LogDebug($"Found solution with id={solutionId}: Solution={solution}, Task={task}");

            var comments = await _repository.Comments
                .Where(e => e.SolutionId == solutionId)
                .OrderByDescending(e => e.CommentDate)
                .Include(e => e.ApplicationUser)
                .ToListAsync();
            if (comments == null || comments?.Count == 0)
            {
                _logger.LogDebug($"There are no comments for solution with id={solutionId}");
            }

            //is it possible to do with one SQL call?
            var counter =_repository.Ratings.Count(r => r.IdSolution == solutionId);
            var rating = _repository.Ratings.Count(r => r.IdSolution == solutionId && r.Value);
            
            return View("Show", new SolutionViewModel(task, solution, comments, rating, counter, 
                solution.Attachments.Select(a => a.File).ToList()));
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> Create(int forTaskId)
        {
            _logger.LogDebug($"Requested solution creation form for task {forTaskId}");

            // Retrieve task from DB
            var task = await _repository.Tasks
                .Include(t => t.Taskset.Course)
                .FirstOrDefaultAsync(t => t.Id == forTaskId);
            if (task == null)
            {
                _logger.LogDebug($"Task not found {forTaskId}, no solution can be added");
                return new StatusCodeResult(400);
            }

            return View("Edit", new SolutionEditModel { Task = task, NewContent = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(String newContent, Data.Entities.Task task,
            List<IFormFile> attachments)
        {
            SolutionEditModel editedSolution = new SolutionEditModel() { NewContent = newContent, Task = task };
            _logger.LogDebug($"Requested to add solution for {editedSolution.Task}; " +
                $"content: length={editedSolution.NewContent?.Length}, hash={editedSolution.NewContent?.GetHashCode()}");
            // Authorize
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, 
                new RolesAuthorizationRequirement(new string[] { UserRoles.TRUSTED_USER }));
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }
            
            // Check if such task exists
            if (await _repository.Tasks.FindAsync(editedSolution.Task.Id) == null)
            {
                _logger.LogDebug($"Task not found {editedSolution.Task.Id}, no solution can be added");
                return new StatusCodeResult(400);
            }

            // Validate
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning(error.ErrorMessage);
                    }
                }
                _logger.LogDebug($"Model state is not valid");
                return new StatusCodeResult(400);
            }

            // Update
            var user = await _userManager.GetUserAsync(HttpContext.User);
            /*  BTW doing it this^ way we assure that user is still in DB, I suppose. */
            var solution = new Solution
            {
                TaskId = editedSolution.Task.Id,
                Author = user,
                CachedContent = Markdown.ToHtml(newContent, _markdownPipeline)
            };
            var version = new SolutionVersion
            {
                Solution = solution,
                Created = DateTime.Now,
                Content = editedSolution.NewContent
            };
            using (var transaction = _repository.Database.BeginTransaction())
            {
                // Now save to database in two steps to avoid circular dependency between foreign keys
                _repository.Solutions.Add(solution);
                _repository.SolutionsVersions.Add(version);
                await _repository.SaveChangesAsync();
                solution.CurrentVersion = version;
                await _repository.SaveChangesAsync();
                transaction.Commit();
            }

            if (newContent.Contains("widać, że"))
            {
                _logger.LogDebug($"Achievement 'Sokole oko' granted for user");
                await _achievementsService.GrantAchievement(user, "SOKOLE OKO");
            }
            var userSolutions = await _repository.Solutions.Where(t => t.Author == user).ToListAsync();
            if (userSolutions.Count >= 1)
            {
                await _achievementsService.GrantAchievement(user, "REDAKTOR I");
            }
            if (userSolutions.Count >= 3)
            {
                await _achievementsService.GrantAchievement(user, "REDAKTOR II");
            }
            if (userSolutions.Count >= 6)
            {
                await _achievementsService.GrantAchievement(user, "REDAKTOR III");
            }
            if (userSolutions.Count >= 10)
            {
                await _achievementsService.GrantAchievement(user, "REDAKTOR IV");
            }
            if (userSolutions.Count >= 20)
            {
                await _achievementsService.GrantAchievement(user, "REDAKTOR V");
            }

            await GrantFirstStepsAchievement();
            await StoreAttachments(solution, attachments);
            return RedirectToAction("Show", new { solutionId = solution.Id });
        }
        
        private async Task StoreAttachments(Solution entity, List<IFormFile> files)
        {
            if (files == null) return;

            // Store files attaching them to taskset
            foreach (var file in files)
            {
                var fileEntity = await _storageService.Store(file.FileName, file.OpenReadStream());
                entity.Attachments.Add(new SolutionsFiles() {SolutionId = entity.Id, FileId = fileEntity.Id});
            }

            await _repository.SaveChangesAsync();
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogDebug($"Requested solution edition form for solution with id {id}");

            // Retrieve task from DB
            var solution = await _repository.Solutions
                .Include(s => s.CurrentVersion)
                .Include(s => s.Task)
                .Include(s => s.Task.Taskset)
                .Include(s => s.Task.Taskset.Course) // TODO something with this ...
                .Where(s => s.Id == id).FirstOrDefaultAsync();
            if (solution == null)
            {
                _logger.LogDebug($"Solution with id {id} not found");
                return new StatusCodeResult(404);
            }

            // Authorize
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, solution, new ModOrOwnerRequirement());
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }
            
            // Return form
            var model = new SolutionEditModel
            {
                Task = solution.Task,
                SolutionId = solution.Id,
                NewContent = solution.CurrentVersion.Content
            };
            return View("Edit", model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit([Bind] SolutionEditModel edited)
        {
            _logger.LogDebug($"Edited SolutionEditModel({edited.Task}, {edited.SolutionId}, {edited.NewContent})");
            if (!edited.ValidForEdit())
            {
                return new StatusCodeResult(400);
            }

            // Load solution
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var solution = await _repository.Solutions.FindAsync(edited.SolutionId);
            if (solution == null)
            {
                return new StatusCodeResult(404);
            }

            // Authorize
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, solution, new ModOrOwnerRequirement());
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            // Create updated version
            var version = new SolutionVersion
            {
                Solution = solution,
                Created = DateTime.Now,
                Content = edited.NewContent
            };

            // Now save to database in two steps to avoid circular dependency between foreign keys
            using (var transaction = _repository.Database.BeginTransaction())
            {
                _repository.SolutionsVersions.Add(version);
                await _repository.SaveChangesAsync();
                solution.CurrentVersion = version;
                solution.CachedContent = Markdown.ToHtml(edited.NewContent, _markdownPipeline);
                await _repository.SaveChangesAsync();
                transaction.Commit();
            }
            return RedirectToAction("Show", new { solutionId = solution.Id });
        }

        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> CreateComment(int forSolutionId)
        {
            var solution = await _repository.Solutions
                .Include(t => t.Task.Taskset.Course)
                .FirstOrDefaultAsync(t => t.Id == forSolutionId);
            if (solution == null)
            {
                _logger.LogDebug($"Solution {forSolutionId} not found, you cannot add comment");
                return new StatusCodeResult(400);
            }
            
            var comment = new Comment
            {
                Content = "",
                Solution = solution,
                SolutionId = forSolutionId
            };
            return View("CreateComment", new CommentViewModel(comment, solution));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("Id,Content, SolutionId, CommentDate")] Comment comment)
        {
            if (await _repository.Solutions.FindAsync(comment.SolutionId) == null)
            {
                _logger.LogDebug($"Solution not found {comment.SolutionId}, no comment can be added");
                return new StatusCodeResult(400);
            }
            // Validate (is content non-empty)
            if (!ModelState.IsValid) 
            {
                _logger.LogDebug($"Model state is not valid");
                return new StatusCodeResult(400);
            }
            // Update
            comment.CommentDate = DateTime.UtcNow;
            comment.CachedContent = Markdown.ToHtml(comment.Content, _markdownPipeline);
            comment.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _repository.Comments.Add(comment);
            await _repository.SaveChangesAsync();
            var commentForThisSolution =
                await _repository.Comments.Where(t => t.SolutionId == comment.SolutionId).ToListAsync();
            var solution = await _repository.Solutions.Where(t => t.Id == comment.SolutionId).FirstOrDefaultAsync();
            var author = solution.Author;
            if (commentForThisSolution.Count == 42 && author!= null)
            {
                //FIXME czy takie wyciąganie autora rozwiązania jest ok?
                await _achievementsService.GrantAchievement(author, "GORĄCY TEMAT");

            }

            await GrantFirstStepsAchievement();


            return RedirectToAction("Show", new { solutionId = comment.SolutionId });

        }

        public async Task GrantFirstStepsAchievement()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userComments = await _repository.Comments.Where(t => t.ApplicationUser == user).ToListAsync();
            var userRatings = await _repository.Ratings.Where(t => t.NameUser == user.Id).ToListAsync();
            var userSolutions = await _repository.Solutions.Where(t => t.Author == user).ToListAsync();
            if (userComments.Count > 0 && userRatings.Count() > 2 && userSolutions.Count() > 0)
            {
                await _achievementsService.GrantAchievement(user, "PIERWSZE KROKI");
            }

            return;
        }
        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        public async Task<IActionResult> AddRating(bool rating, int solutionId)
        {
            _logger.LogDebug($"Requested to add rating for {solutionId}; ");

            //sprawdzamy czy istnieje rozwiazanie do którego chcemy dodać ocenę
            var solution = await _repository.Solutions.FindAsync(solutionId);
            if (solution == null)
            {
                _logger.LogDebug($"Solution not found {solutionId}, no rating can be added");
                return new StatusCodeResult(400);
            }

            //sprawdzamy czy już oceniał wcześniej
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var oldRating = await _repository.Ratings.Where(r => r.IdSolution == solutionId && r.NameUser == userId).ToListAsync();
            if(oldRating.Count == 0){
                _repository.Ratings.Add(new Rating{IdSolution=solutionId, NameUser=userId, Value=rating});
                await _repository.SaveChangesAsync();
            }
            else{
                //wystarczy zrobić tak, nie trzeba robić żadnej magii w stylu usuwanie poprzedniego rekordu
                //i dodawanie nowego
                oldRating[0].Value = rating;
                await _repository.SaveChangesAsync();
            }

            await GrantFirstStepsAchievement();
            // Validate
            if (!ModelState.IsValid)
            {
                _logger.LogDebug($"Model state is not valid");
                return new StatusCodeResult(400);
            }


            return RedirectToAction("Show", new { solutionId = solutionId });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var solution = await _repository.Solutions.Include(s => s.Task)
                .Where(s => s.Id == id).FirstOrDefaultAsync();
            if (solution == null)
            {
                _logger.LogDebug($"Solution(Id={id}) not found");
                return new StatusCodeResult(404);
            }

            // Authorize
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, solution, new ModOrOwnerRequirement());
            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            _repository.Solutions.Remove(solution);
            await _repository.SaveChangesAsync();
            return RedirectToAction("ShowTaskset", "Taskset", new { id = solution.Task.TasksetId });
        }
    }
}
