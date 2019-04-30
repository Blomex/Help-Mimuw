using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Data.Enums;
using archive.Models;
using archive.Models.Comment;
using archive.Models.Solution;
using archive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace archive.Controllers
{
    public class SolutionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public SolutionController(ILogger<SolutionController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
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

            var solution = new Solution
            {
                Content = "Tu wpisz tresć rozwiązania...",
                TaskId = forTaskId
            };

            return View("Create", new SolutionViewModel(task, solution, new List<Comment>(), 0, 0));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.TRUSTED_USER)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskId,Content")] Solution solution)
        {
            _logger.LogDebug($"Requested to add solution for {solution.TaskId}; " +
                $"content: length={solution.Content?.Length}, hash={solution.Content?.GetHashCode()}");
            
            // Check if such task exists
            if (await _repository.Tasks.FindAsync(solution.TaskId) == null)
            {
                _logger.LogDebug($"Task not found {solution.TaskId}, no solution can be added");
                return new StatusCodeResult(400);
            }

            // Validate
            if (!ModelState.IsValid)
            {
                _logger.LogDebug($"Model state is not valid");
                return new StatusCodeResult(400);
            }

            // Update
            _repository.Solutions.Add(solution);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Show", new { solutionId = solution.Id });
        }

        [Authorize]
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
        [Authorize]
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
            comment.CommentDate = DateTime.Now;
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

    }
}
