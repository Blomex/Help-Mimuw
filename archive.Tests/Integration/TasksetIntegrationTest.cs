using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Enums;
using archive.Models.Taskset;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

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
                new CreateTasksetViewModel {Type = TasksetType.Other.ToString(), Name = name, CourseId = 1});

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, redirect.StatusCode);
            Assert.True(redirect.Headers.Location.OriginalString.StartsWith("/Taskset/Index"));
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                
                var repository = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//                repository.Users.
                var added = repository.Tasksets.Where(t => t.Name == name).ToList();
                Assert.Equals(1, added.Count);
                Assert.Equals(name, added[0].Name);
//                Assert.Equals()
            }
        }
    }
}