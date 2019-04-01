using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using archive.Data;
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
    }
}