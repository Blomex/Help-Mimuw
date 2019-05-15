using archive.Data;
using archive.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using archive.Data.Entities;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using archive.Controllers;
using archive.Models.Solution;

namespace archive.Tests.Controllers
{
    class SolutionControllerTest : ControllerTestSuite
    {
        private Course course;
        private Taskset taskset;
        private Data.Entities.Task task;


        [OneTimeSetUp]
        public void SetUp()
        {
            // Prepare entities
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                course = new Course
                {
                    Name = "Golemnacja"
                };
                db.Courses.Add(course);

                taskset = new Taskset
                {
                    Course = course,
                    Name = "Roczny Konkurs",
                    Type = TasksetType.Exam,
                    Year = 1322
                };
                db.Tasksets.Add(taskset);

                task = new Data.Entities.Task
                {
                    Taskset = taskset,
                    Name = "Autodestrukcja",
                    Content = "Golem ma sam się zniszczyć"
                };
                db.Tasks.Add(task);

                db.SaveChanges();
            }
        }

        [Test]
        public async Task TrustedUserCanAddSolution()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var controller = LoginToController<SolutionController>(scope, marielle);
                const string content = "Nasz golem wybucha.";

                var createResult = await controller.Create(content, task) as RedirectToActionResult;
                Assert.NotNull(createResult != null);
                Assert.AreEqual(createResult.ActionName, "Show");
                var id = (int)createResult.RouteValues["solutionId"];
                Assert.Greater(id, 0);

                var showResult = await controller.Show(id) as ViewResult;
                Assert.NotNull(showResult);
                Assert.AreEqual(showResult.ViewName, "Show");
                var model = showResult.Model as SolutionViewModel;
                Assert.NotNull(model);
                Assert.AreEqual(model.Task.TasksetId, task.TasksetId);
                Assert.AreEqual(model.Task.Name, task.Name);
            }
        }

        [Test]
        public async Task UntrustedUserCannotAddSolution()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var controller = LoginToController<SolutionController>(scope, lenesia);
                const string content = "Nasz golem wybucha.";

                var r = await controller.Create(content, task);
                var createResult = r as ForbidResult;
                Assert.NotNull(createResult);
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Courses.Remove(course);
                db.Tasksets.Remove(taskset);
                db.SaveChanges();
            }
        }

        
    }
}
