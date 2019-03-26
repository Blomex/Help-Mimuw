using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using archive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace archive.Controllers
{
    public class SolutionController : Controller
    {
        private readonly ILogger _logger;

        public SolutionController(ILogger<SolutionController> logger)
        {
            _logger = logger;
        }

        public IActionResult Show(int solutionId)
        {
            _logger.LogDebug($"Requested solution for {solutionId}");
            SolutionViewModel model = new SolutionViewModel();
            model.TaskName = "Zadanie " + solutionId;
            return View("Show", model);
        }
    }
}