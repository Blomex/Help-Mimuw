using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using archive.Data;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace archive.Tests.Integration
{
    [TestFixture]
    public class CourseIntegrationTest
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
        public async Task GetAllCourses()
        {
            // Act
            var coursesResponse = await _client.GetAsync("/");
            Assert.AreEqual(HttpStatusCode.OK, coursesResponse.StatusCode);
            var coursesHtmlDocument = await HtmlHelpers.GetDocumentAsync(coursesResponse);

            // Assert
            var main = coursesHtmlDocument.GetElementsByTagName("main");
            Assert.AreEqual(1, main.Length);
            
            var obtainedCourses = new List<string>();
            foreach (var a in main.Children("a"))
            {
                obtainedCourses.Add(a.TextContent);
            }

            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var realCourses = repository.Courses.Select(c => c.Name).ToList();
                realCourses.Sort();
                obtainedCourses.Sort();

                for (int i = 0; i < realCourses.Count; ++i)
                {
                    Assert.AreEqual(realCourses[i], obtainedCourses[i].Trim());
                }
            }
        }
    }
}