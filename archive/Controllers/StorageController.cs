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
    public class StorageController : AbstractArchiveController
    {
        protected IStorageService _storageService { get; }
        protected ILogger<StorageController> _logger { get; }

        public StorageController(ILogger<StorageController> logger, IStorageService storageService,
            IUserActivityService userActivityService)
            : base(userActivityService)
        {
            _storageService = storageService;
            _logger = logger;
        }

        [Authorize]
        public async Task<ActionResult> Index(string id)
        {
            _logger.LogDebug($"Working in directory '{Directory.GetCurrentDirectory()}'");

            Guid fileGuid;
            try
            {
                if (id == null)
                    throw new ArgumentNullException();
                var i = id.IndexOf('.');
                var guidString = i > 0 ? id.Substring(0, i) : id;
                fileGuid = new Guid(guidString);
                _logger.LogDebug($"File(GUID={fileGuid}) requested");
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException || e is FormatException || e is OverflowException)
                    return new StatusCodeResult(404);
                throw;
            }

            var file = await _storageService.Retrieve(fileGuid);
            if (file == null)
                return new StatusCodeResult(404);
            _logger.LogDebug($"Stream file from '{file.Path}'");
            return File(System.IO.File.OpenRead(file.Path), file.MimeType + "/" + file.MimeSubtype, file.FileName);
        }
    }
}