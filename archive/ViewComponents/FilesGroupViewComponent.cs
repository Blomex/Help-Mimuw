using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Services.Storage;
using Microsoft.AspNetCore.Mvc;

namespace archive.ViewComponents
{
    public class FilesGroupViewComponent : ViewComponent
    {
        private readonly IStorageService _storage;

        public FilesGroupViewComponent(IStorageService service)
        {
            _storage = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid baseGroupGuid)
        {
            var baseGroupFiles = baseGroupGuid == Guid.Empty ? await _storage.FilesFromGroup(baseGroupGuid) : new HashSet<File>();
            return View(baseGroupFiles);
        }
    }
}
