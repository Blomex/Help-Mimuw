using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace archive.Models.Solution
{
    public class AttachmentEditModel
    {
        public Guid BaseGroup = Guid.Empty;
        public string RemovedFiles { get; set; }
        public List<List<IFormFile>> UploadedFiles { get; set; } = new List<List<IFormFile>>();
    }

    public class SolutionEditModel
    {
        public Data.Entities.Task Task { get; set; }
        public int? SolutionId { get; set; }
        public string NewContent { get; set; }
        public AttachmentEditModel Attachments { get; set; } = new AttachmentEditModel();

        public bool ValidForEdit()
        {
            return NewContent != null && NewContent.Length > 1 && SolutionId != null && SolutionId > 0;
        }
    }
}
