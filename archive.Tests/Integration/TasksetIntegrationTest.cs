using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using archive.Data;
using archive.Data.Entities;
using archive.Data.Enums;
using archive.Models.Taskset;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Task = System.Threading.Tasks.Task;

namespace archive.Tests.Integration
{
    [TestFixture]
    public class TasksetIntegrationTest
    {
        private WebApplicationFactory<StartupWithoutAuthentication> _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void InitApplication()
        {
            _factory = new WebApplicationFactory<StartupWithoutAuthentication>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task ShowTasksets()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                // Arrange
                var repository = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var name1 = Guid.NewGuid().ToString();
                var name2 = Guid.NewGuid().ToString();
                var taskset1 = new Taskset {Name = name1, Type = TasksetType.Exam, Year = 2000, CourseId = 1};
                var taskset2 = new Taskset {Name = name2, Type = TasksetType.Test, Year = 2001, CourseId = 1};
                repository.Tasksets.Add(taskset1);
                repository.Tasksets.Add(taskset2);
                repository.SaveChanges();
                
                // Act
                var tasksetsResponse = await _client.GetAsync("/Taskset/Index/1");
                var tasksetsHtmlDocument = await HtmlHelpers.GetDocumentAsync(tasksetsResponse);
                Assert.AreEqual(HttpStatusCode.OK, tasksetsResponse.StatusCode);

                // Assert
                var main = tasksetsHtmlDocument.GetElementsByTagName("main");
                Assert.AreEqual(1, main.Length);

                var expectedTasksets = repository.Tasksets
                    .Where(t => t.CourseId == 1)
                    .Select(t => t.Name)
                    .ToList();
                repository.Remove(taskset1);
                repository.Remove(taskset2);
                repository.SaveChanges();
                
                var obtainedTasksets = new List<string>();
                foreach (var a in main.Children("li a"))
                {
                    obtainedTasksets.Add(a.TextContent);
                }
                
                obtainedTasksets.Sort();
                expectedTasksets.Sort();

                for (int i = 0; i < obtainedTasksets.Count; ++i)
                {
                    Assert.AreEqual(expectedTasksets[i], obtainedTasksets[i].Trim());
                }
            }
        }

        [Test]
        public async Task ShowTasks()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                // Arrange
                var repository = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var tasksetName1 = Guid.NewGuid().ToString();
                var taskset1 = new Taskset {Name = tasksetName1, Type = TasksetType.Exam, Year = 2000, CourseId = 1};
                repository.Tasksets.Add(taskset1);
                repository.SaveChanges();

                var taskName1 = Guid.NewGuid().ToString();
                var taskName2 = Guid.NewGuid().ToString();
                var content = "Testowe zadanie";
                var task1 = new Data.Entities.Task {Name = taskName1, Content = content, TasksetId = taskset1.Id};
                var task2 = new Data.Entities.Task {Name = taskName2, Content = content, TasksetId = taskset1.Id};
                repository.Tasks.Add(task1);
                repository.Tasks.Add(task2);
                repository.SaveChanges();

                // Act
                var tasksResponse = await _client.GetAsync("/Taskset/ShowTaskset/" + taskset1.Id);
                var tasksHtmlDocument = await HtmlHelpers.GetDocumentAsync(tasksResponse);
                Assert.AreEqual(HttpStatusCode.OK, tasksResponse.StatusCode);

                // Assert
                var main = tasksHtmlDocument.GetElementsByTagName("main");
                Assert.AreEqual(1, main.Length);

                repository.Remove(taskset1);
                repository.Remove(task1);
                repository.Remove(task2);
                repository.SaveChanges();

                var obtainedTasksets = new List<string>();
                foreach (var a in main.Children("pre"))
                {
                    Assert.AreEqual(content, a.TextContent);
                }
            }
        }

        [Test]
        public async Task CreateTaskset()
        {
            // Arrange
            var name = Guid.NewGuid().ToString();
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });


            // Act
            var redirect = await client.PostAsJsonAsync("/Taskset/Create",
                new CreateTasksetViewModel
                    {Type = TasksetType.Other.ToString(), Year = 2000, Name = name, CourseId = 1});

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, redirect.StatusCode);
            Assert.True(redirect.Headers.Location.OriginalString.StartsWith("/Taskset/Index"));
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var added = repository.Tasksets.Where(t => t.Name == name).ToList();

                Assert.Equals(1, added.Count);
                repository.Tasksets.Remove(added[0]);
                repository.SaveChanges();

                Assert.Equals(name, added[0].Name);
                Assert.Equals(1, added[0].CourseId);
                Assert.Equals(TasksetType.Exam, added[0].Type);
                Assert.Equals(2000, added[0].Year);
            }
        }
    }
}