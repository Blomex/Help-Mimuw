using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace archive.Tests.Integration
{
    [TestFixture]
    public class UrlMappingTest
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        
        [OneTimeSetUp]
        public void InitApplication()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        [Test]
        [TestCase("/")]
        
        [TestCase("/Taskset/Index/1")]
        [TestCase("/Taskset/ShowTaskset/1")]
        [TestCase("/Taskset/Create")]
        
        [TestCase("/Task/Create")]
        [TestCase("/Task/Create?forTasksetId=1")]
        
        [TestCase("/solution/1")]
        [TestCase("/solution/create/1")]
        [TestCase("/solution/addrating/1?rating=true")]
        [TestCase("/solution/addrating/1?rating=false")]
        [TestCase("/Solution/CreateComment?forSolutionId=1")]
        
        [TestCase("/Identity/Account/Login")]
        [TestCase("/Identity/Account/Register")]
        [TestCase("/Identity/Account/ForgotPassword")]
        [TestCase("/Identity/Account/Manage")]
        public async Task EnterMappedPath(string path)
        {
            // Act
            var response = await _client.GetAsync(path);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        // FIXME unifikacja sciezek (jest na to ticket)
        [Test]
        [TestCase("/Taskset/Create")]
        
        [TestCase("/Task/Create")]  
        [TestCase("/Task/Create?forTasksetId=1")]
        
        [TestCase("/solution/create/1")]
        [TestCase("/solution/addrating/1?rating=true")]
        [TestCase("/solution/addrating/1?rating=false")]
        [TestCase("/Solution/CreateComment?forSolutionId=1")]
        [TestCase("/Identity/Account/Manage")]
        public async Task RedirectToLoginPage(string path)
        {
            // Arrange
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Act
            var response = await client.GetAsync(path);

            // Assert
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
            Assert.True(response.Headers.Location.OriginalString.StartsWith(
                "http://localhost/Identity/Account/Login"));
        }
    }
}