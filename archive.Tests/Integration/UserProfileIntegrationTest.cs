using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using archive.Data;
using archive.Data.Entities;
using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Task = System.Threading.Tasks.Task;

namespace archive.Tests.Integration
{
    [TestFixture]
    public class UserProfileIntegrationTest
    {
        private WebApplicationFactoryWithoutAuthentication _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void InitApplication()
        {
            _factory = new WebApplicationFactoryWithoutAuthentication();
            _client = _factory.CreateClient();
        }

        [Test]
        [Ignore("Require explicit login or changes in implementation")]
        public async Task EditProfile()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                // Arrange
                var repository = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var date = DateTime.UtcNow;
                var user = new ApplicationUser()
                {
                    HomePage = "mimuw.edu.pl",
                    LastActive = date,
                    PhoneNumber = "123456789",
                    Email = "a@b.c"
                };
                await repository.Users.AddAsync(user);
                await repository.SaveChangesAsync();

                // Act
                var content = new MultipartFormDataContent();
                var dataContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Input.Email", "a@b.cd"),
                    new KeyValuePair<string, string>("Input.PhoneNumber", "987654321"),
                    new KeyValuePair<string, string>("Input.HomePage", "students.mimuw.edu.pl"),
                });
                content.Add(dataContent);

                var response = await _client.PostAsync("/Identity/Account/Manage", content);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                // Assert
                Assert.AreEqual("a@b.c", user.Email);
                Assert.AreEqual("987654321", user.PhoneNumber);
                Assert.AreEqual("students.mimuw.edu.pl", user.HomePage);
                repository.Users.Remove(user);
            }
        }

        [Test]
        public async Task ShowProfile()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                // Arrange
                var repository = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var date = DateTime.UtcNow;
                var name = Guid.NewGuid().ToString();
                var user = new ApplicationUser()
                {
                    HomePage = "mimuw.edu.pl",
                    LastActive = date,
                    PhoneNumber = "123456789",
                    Email = "a@b.c",
                    UserName = name
                };
                await repository.Users.AddAsync(user);
                await repository.SaveChangesAsync();

                // Act
                var response = await _client.GetAsync("/User/ShowProfile?name=" + user.UserName);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                var document = await HtmlHelpers.GetDocumentAsync(response);

                // Assert
                var main = document.GetElementsByTagName("main");
                Assert.AreEqual(1, main.Length);

                var properties = main.Children("pre").ToArray();
                Assert.AreEqual(5, properties.Length);
                Assert.AreEqual(user.UserName, properties[0].TextContent.Trim());
                Assert.AreEqual(user.HomePage, properties[1].TextContent.Trim());
                Assert.AreEqual(user.Email, properties[2].TextContent.Trim());
                Assert.AreEqual(user.PhoneNumber, properties[3].TextContent.Trim());
                var culture = new CultureInfo("en-US");
                var resultDate = Convert.ToDateTime(properties[4].TextContent.Trim(), culture);
                Assert.AreEqual(user.LastActive.AddHours(2).Year, resultDate.Year);
                Assert.AreEqual(user.LastActive.AddHours(2).Month, resultDate.Month);
                Assert.AreEqual(user.LastActive.AddHours(2).Day, resultDate.Day);
                Assert.AreEqual(user.LastActive.AddHours(2).Hour, resultDate.Hour);
                Assert.AreEqual(user.LastActive.AddHours(2).Minute, resultDate.Minute);
                Assert.AreEqual(user.LastActive.AddHours(2).Second, resultDate.Second);
                repository.Users.Remove(user);
            }
        }
    }
}