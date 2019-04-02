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
            var tasksets = await _repository.Tasksets.Where(s => s.CourseId == id).OrderByDescending(s => s.Year).ToListAsync();

            var model = new IndexViewModel {Tasksets = tasksets};
            return View("Index", model);
        }

        public async Task<IActionResult> ShowTaskset(int id)
        {
            var taskset = await _repository.Tasksets.FindAsync(id);

            if (taskset == null)
            {
                return new StatusCodeResult(404);
            }


            var tasks = await _repository.Tasks.Where(s => s.TasksetId == id).ToListAsync();

            var listOfSolutions = new Dictionary<int, List<Solution>>();

            foreach (var task in tasks) {
                var solutions = await _repository.Solutions.Where(s => s.TaskId == task.Id).ToListAsync();
                listOfSolutions.Add(task.Id, solutions);
            }

            var model = new TasksetViewModel {Taskset = taskset, Tasks = tasks, ListOfSolutions = listOfSolutions};

            return View("ShowTaskset", model);
        }
    }
}