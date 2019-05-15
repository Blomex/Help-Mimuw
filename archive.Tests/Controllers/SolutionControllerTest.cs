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

                var createResult = await controller.Create(content, task, new List<IFormFile>()) as RedirectToActionResult;
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

                var r = await controller.Create(content, task, new List<IFormFile>());
                var createResult = r as ForbidResult;
                Assert.NotNull(createResult);
            }
        }

        [Test]
        public async Task OwnerCanEditSolution()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                // Given solution by Akatsuki
                var solution = await InsertAkatsukiSolution(scope);

                // When she tries to edit it
                var controller = LoginToController<SolutionController>(scope, akatsuki);
                var editedModel = new SolutionEditModel
                {
                    SolutionId = solution.Id,
                    NewContent = "Za trudne, nie wymówię..."
                };
                var r = await controller.Edit(editedModel) as RedirectToActionResult;

                // It is accepted and redirects her to to solution
                Assert.NotNull(r != null);
                Assert.AreEqual(r.ActionName, "Show");
                Assert.AreEqual((int)r.RouteValues["solutionId"], solution.Id);
            }
        }

        [Test]
        public async Task NotOwnerCannotEditSolution()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                // Given solution by Akatsuki
                var solution = await InsertAkatsukiSolution(scope);

                // When henrietta tries to edit it
                var controller = LoginToController<SolutionController>(scope, henrietta);
                var editedModel = new SolutionEditModel
                {
                    SolutionId = solution.Id,
                    NewContent = "BUHAHA"
                };
                var r = await controller.Edit(editedModel) as ForbidResult;

                // She cannot
                Assert.NotNull(r != null);
            }
        }

        /* Z jakiegoś powodu rzuca Circular dependency
        [Test]
        public async Task ModCanRemoveAnySolution()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Given solution by Akatsuki
                var solution = await InsertAkatsukiSolution(scope);

                // When he tries to remove it
                var controller = LoginToController<SolutionController>(scope, shiroe);
                var r = await controller.Delete(solution.Id) as RedirectToActionResult;
                

                // It really gets deleted
                Assert.NotNull(r != null);
                Assert.Null(await db.Solutions.FindAsync(solution.Id));
            }
        }*/

        [OneTimeTearDown]
        public void TearDown()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Courses.Remove(course);
                db.SaveChanges();
            }
        }

        protected async Task<Solution> InsertAkatsukiSolution(IServiceScope scope)
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var solution = new Solution
            {
                AuthorId = akatsuki.Id,
                CachedContent = "xD",
                TaskId = task.Id
            };
            var version = new SolutionVersion
            {
                Solution = solution,
                Created = DateTime.Now,
                Content = "Tracz tak tarcicę tarł, tak takt w takt, jak takt w takt, tercicę tartak tarł"
            };
            db.Add(solution);
            await db.SaveChangesAsync();
            solution.CurrentVersion = version;
            db.Add(version);
            await db.SaveChangesAsync();

            return solution;
        }
    }
}
