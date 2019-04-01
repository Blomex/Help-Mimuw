using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Models;
using archive.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

            var task = await _repository.Tasks.FindAsync(solution.TaskId);
            if (task == null)
            {
                _logger.LogWarning($"Solution with id={solutionId} exists but references task with " +
                    $"id={solution.TaskId} which does not");
                return new StatusCodeResult(404); // Surely?
            }
            _logger.LogDebug($"Found solution with id={solutionId}: Solution={solution}, Task={task}");

            return View("Show", new SolutionViewModel(task, solution));
        }

        public async Task<IActionResult> Create(int forTaskId)
        {
            _logger.LogDebug($"Requested solution creation form for task {forTaskId}");

            // Retrieve task from DB
            var task = await _repository.Tasks.FindAsync(forTaskId);
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

            return View("Create", new SolutionViewModel(task, solution));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskId,Content")] Solution solution)
        {
            /* Czemu Bind działa bez `Solution.`? */
            _logger.LogDebug($"Requested to add solution for {solution.TaskId}; " +
                $"content: length={solution.Content.Length}, hash={solution.Content.GetHashCode()}");
            
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
    }
}