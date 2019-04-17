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
using Task = System.Threading.Tasks.Task;

namespace archive.Tests.Integration
{
    [TestFixture]
    public class TasksetIntegrationTest
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new WebApplicationFactory<Startup>();
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
        [Ignore("Require being logged -- unsupported yet.")]
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