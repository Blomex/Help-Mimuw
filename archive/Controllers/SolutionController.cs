using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Models;
using archive.Models.Comment;
using archive.Models.Solution;
using archive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;
using Microsoft.AspNet.Identity;

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
            //just to check if they are seen correctly
            return View("Show", new SolutionViewModel(task, solution, comments));
        }

        [Authorize]
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
                Content = "Tu wpisz tresć rowziązania...",
                TaskId = forTaskId
            };

            List<Comment> a = new List<Comment>();
            return View("Create", new SolutionViewModel(task, solution,a));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskId,Content")] Solution solution)
        {
            /* Czemu Bind działa bez `Solution.`? */
            _logger.LogDebug($"Requested to add solution for {solution.TaskId}; " +
                $"content: length={solution.Content?.Length}, hash={solution.Content?.GetHashCode()}");
            
            // Check if such task exists
            if (await _repository.Tasks.FindAsync(solution.TaskId) == null)
            {
                _logger.LogDebug($"Task not found {solution.TaskId}, no solution can be added");
                return new StatusCodeResult(400);
            }

            // Validate
            if (!ModelState.IsValid) /* huh? */
            {
                _logger.LogDebug($"Model state is not valid");
                return new StatusCodeResult(400);
            }

            // Update
            _repository.Solutions.Add(solution);
            await _repository.SaveChangesAsync(); /* FIXME Can it fail? */
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
                solution = _repository.Solutions.Find(forSolutionId);
                _logger.LogDebug($"Solution {forSolutionId} not found, you cannot add comment");
                return new StatusCodeResult(400);
            }
            
            var comment = new Comment
            {
                content = "",
                Solution = solution,
                SolutionId = forSolutionId
            };
            return View("CreateComment", new CommentViewModel(comment, solution));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("Id,content, SolutionId, CommentDate")] Comment comment)
        {
            if (await _repository.Solutions.FindAsync(comment.SolutionId) == null)
            {
                _logger.LogDebug($"Solution not found {comment.SolutionId}, no comment can be added");
                return new StatusCodeResult(400);
            }
            //Solution exists, so we can add new comment
            // Validate
            if (!ModelState.IsValid) /* huh? */
            {
                _logger.LogDebug($"Model state is not valid");
                return new StatusCodeResult(400);
            }
            // Update
            comment.CommentDate = DateTime.Now;
            comment.ApplicationUserId = User.Identity.GetUserId();
            _repository.Comments.Add(comment);
            await _repository.SaveChangesAsync(); /* FIXME Can it fail? */
            return RedirectToAction("Show", new { solutionId = comment.SolutionId });

        }

    }
}