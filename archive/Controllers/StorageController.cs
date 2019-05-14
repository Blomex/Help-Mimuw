using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archive.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace archive.Controllers
{
    public class StorageController : Controller
    {
        protected StorageService storageService_ { get; }
        protected ILogger<StorageController> logger_ { get; }

        public StorageController(ILogger<StorageController> logger, StorageService storageService)
        {
            storageService_ = storageService;
            logger_ = logger;
        }

        [Authorize]
        public async Task<ActionResult> Index(string id)
        {
            logger_.LogDebug($"Working in directory '{Directory.GetCurrentDirectory()}'");

            Guid fileGuid;
            try
            {
                if (id == null)
                    throw new ArgumentNullException();
                var i = id.IndexOf('.');
                var guidString = i > 0 ? id.Substring(0, i) : id;
                fileGuid = new Guid(guidString);
                logger_.LogDebug($"File(GUID={fileGuid}) requested");
            }
            catch (Exception e) 
            {
                if (e is ArgumentNullException || e is FormatException || e is OverflowException)
                    return new StatusCodeResult(404);
                throw;
            }

            var file = await storageService_.Retrieve(fileGuid);
            if (file == null)
                return new StatusCodeResult(404);
            logger_.LogDebug($"Stream file from '{file.Path}'");
            return File(System.IO.File.OpenRead(file.Path), file.MimeType + "/" + file.MimeSubtype, file.FileName);
        }
    }
}
