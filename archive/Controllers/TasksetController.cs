using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Models.Taskset;
using archive.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.EntityFrameworkCore;

namespace archive.Controllers
{
    public class TasksetController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public TasksetController(ILogger<TasksetController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<IActionResult> Index(int id)
        {
            _logger.LogDebug($"Requested taskset for course id={id}");
            var tasksets = await _repository.Tasksets.Where(s => s.CourseId == id).ToListAsync();
            return View("Index", tasksets);
        }

        public async Task<IActionResult> ShowTaskset(int id)
        {
            var taskset = await _repository.Tasksets.FindAsync(id);

            if (taskset == null)
            {
                return new StatusCodeResult(404);
            }

            var tasks = await _repository.Tasks.Where(s => s.TasksetId == id).ToListAsync();

            var model = new TasksetViewModel {Taskset = taskset, Tasks = tasks};

            return View("ShowTaskset", model);
        }
    }
}