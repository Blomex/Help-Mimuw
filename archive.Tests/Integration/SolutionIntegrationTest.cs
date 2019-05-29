using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using archive.Data;
using archive.Data.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Task = System.Threading.Tasks.Task;

namespace archive.Tests.Integration
{
    public class SolutionIntegrationTest
    {
        private WebApplicationFactoryWithoutAuthentication _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void InitApplication()
        {
            _factory = new WebApplicationFactoryWithoutAuthentication();
            _client = _factory.CreateClient();
        }

        [Test] public async Task ShowSolutionWithComments()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                // Arrange
                var repository = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var addedUser = new ApplicationUser()
                {
                    HomePage = "mimuw.edu.pl",
                    LastActive = DateTime.UtcNow,
                    PhoneNumber = "123456789",
                    Email = "a@b.c",
                    UserName = Guid.NewGuid().ToString()
                };
                await repository.Users.AddAsync(addedUser);
                await repository.SaveChangesAsync();
                var user = await repository.Users.FirstOrDefaultAsync();
                var solution = new Solution {CachedContent = "Indukcja po $n$.", TaskId = 1};
                repository.Solutions.Add(solution);
                await repository.SaveChangesAsync();
                var comment1 = new Comment {Content = "Fajne", CachedContent = "Fajne", SolutionId = solution.Id, ApplicationUserId = user.Id};
                var comment2 = new Comment {Content = "Niefajne", CachedContent = "Niefajne", SolutionId = solution.Id, ApplicationUserId = user.Id};
                repository.Comments.Add(comment1);
                repository.Comments.Add(comment2);
                await repository.SaveChangesAsync();

                // Act
                var solutionResponse = await _client.GetAsync("/solution/" + solution.Id);
                var solutionHtmlDocument = await HtmlHelpers.GetDocumentAsync(solutionResponse);
                Assert.AreEqual(HttpStatusCode.OK, solutionResponse.StatusCode);
                
                // Assert
                var pres = solutionHtmlDocument.GetElementsByClassName("Solution");
                Assert.AreEqual(1, pres.Length);
                Assert.AreEqual("Indukcja po $n$.", pres[0].TextContent.Trim());

                var commentContents = solutionHtmlDocument
                    .GetElementsByClassName("ContentHere")
                    .Select(c => c.TextContent.Trim())
                    .OrderBy(c => c)
                    .ToList();
                
                Assert.AreEqual(2, commentContents.Count);
                Assert.AreEqual("Fajne", commentContents[0]);
                Assert.AreEqual("Niefajne", commentContents[1]);
            }
        }
    }
}