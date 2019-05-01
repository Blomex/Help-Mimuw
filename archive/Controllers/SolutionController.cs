﻿using System;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace archive.Controllers
{
    public class SolutionController : ArchiveController
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public SolutionController(ILogger<SolutionController> logger, ApplicationDbContext repository, 
            UserManager<ApplicationUser> userManager, IUserActivityService activityService,
            IAuthorizationService authorizationService) : base(activityService)
        {
            _logger = logger;
            _repository = repository;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [Authorize]
        public async Task<IActionResult> Show(int solutionId)
        {
            _logger.LogDebug($"Requested solution with id={solutionId}");

            var solution = await _repository.Solutions.FindAsync(solutionId);
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
            
            var rating_list = await _repository.Ratings.Where(r => r.IdSolution == solutionId).ToListAsync();
            int rating = 0;
            int counter = 0;
            //sumujemy oceny
            foreach (var r in rating_list)
            {
                //może jednak zła ocena to 0 zamiast 1?
                // i podawanie rzeczy w stylu '85% osób uważa to rozwiązanie za dobre'
                if(r.Value){rating++;}
                counter++;
            }

            //just to check if they are seen correctly
            return View("Show", new SolutionViewModel(task, solution, comments, rating, counter));
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
        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(String NewContent, Data.Entities.Task Task)
        {
            SolutionEditModel edited_solution = new SolutionEditModel() { NewContent = NewContent, Task = Task };
            _logger.LogDebug($"Requested to add solution for {edited_solution.Task}; " +
                $"content: length={edited_solution.NewContent?.Length}, hash={edited_solution.NewContent?.GetHashCode()}");
            
            // Check if such task exists
            if (await _repository.Tasks.FindAsync(edited_solution.Task.Id) == null)
            {
                _logger.LogDebug($"Task not found {edited_solution.Task.Id}, no solution can be added");
                return new StatusCodeResult(400);
            }

            // Validate
            if (!ModelState.IsValid)
            {
                _logger.LogDebug($"Model state is not valid");
                return new StatusCodeResult(400);
            }

            // Update
            var user = await _userManager.GetUserAsync(HttpContext.User);
            /*  BTW doing it this^ way we assure that user is still in DB, I suppose. */
            var solution = new Solution
            {
                TaskId = edited_solution.Task.Id,
                Author = user,
                CachedContent = NewContent
            };
            var version = new SolutionVersion
            {
                Solution = solution,
                Created = DateTime.Now,
                Content = edited_solution.NewContent
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
            return RedirectToAction("Show", new { solutionId = solution.Id });
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
                solution.CachedContent = edited.NewContent;
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
            comment.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _repository.Comments.Add(comment);
            await _repository.SaveChangesAsync(); /* FIXME Can it fail? */
            return RedirectToAction("Show", new { solutionId = comment.SolutionId });

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
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var old_rating = await _repository.Ratings.Where(r => r.IdSolution == solutionId && r.NameUser == userID).ToListAsync();
            if(old_rating.Count == 0){
                _repository.Ratings.Add(new Rating{IdSolution=solutionId, NameUser=userID, Value=rating});
                await _repository.SaveChangesAsync();
            }
            else{
                //wystarczy zrobić tak, nie trzeba robić żadnej magii w stylu usuwanie poprzedniego rekordu
                //i dodawanie nowego
                old_rating[0].Value = rating;
                await _repository.SaveChangesAsync();

            }
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
