using archive.Controllers;
using archive.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using archive.Services.Storage;

namespace archive.Tests.Controllers
{
    class StorageControllerTest : ControllerTestSuite
    {
        [Test]
        public async Task CanStoreFile()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var storage = scope.ServiceProvider.GetRequiredService<IStorageService>();
                var storedContent = "I'm a palindrome!";
                var stream = new MemoryStream(Encoding.ASCII.GetBytes(storedContent));

                var file = await storage.Store("tacocat.txt", stream, "text/plain");

                var savedContent = File.ReadAllText(file.Path);
                Assert.AreEqual(savedContent, storedContent);
                Assert.AreEqual(file.MimeType, "text");
                Assert.AreEqual(file.MimeSubtype, "plain");
            }
        }

        [Test]
        public async Task CanRetrieveStoredFile()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var storage = scope.ServiceProvider.GetRequiredService<IStorageService>();
                var storedContent = "I'm a palindrome!";
                var stream = new MemoryStream(Encoding.ASCII.GetBytes(storedContent));

                var f = await storage.Store("tacocat.txt", stream, "text/plain");
                var file = await storage.Retrieve(f.Id);

                var savedContent = File.ReadAllText(file.Path);
                Assert.AreEqual(savedContent, storedContent);
                Assert.AreEqual(file.MimeType, "text");
                Assert.AreEqual(file.MimeSubtype, "plain");
            }
        }

        [Test]
        public async Task CanDownloadFile()
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var storage = scope.ServiceProvider.GetRequiredService<IStorageService>();
                var storedContent = "I'm a palindrome!";
                var stream = new MemoryStream(Encoding.ASCII.GetBytes(storedContent));

                var f = await storage.Store("tacocat.txt", stream, "text/plain");
                var controller = LoginToController<StorageController>(scope, akatsuki);
                var response = await controller.Index(f.Id.ToString() + ".txt") as FileResult;

                Assert.NotNull(response);
                Assert.AreEqual(response.ContentType, "text/plain");
                Assert.AreEqual(response.FileDownloadName, "tacocat.txt");
            }
        }
    }
}
