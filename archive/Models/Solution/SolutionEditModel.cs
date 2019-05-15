using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using archive.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace archive.Models.Solution
{
    public class SolutionEditModel
    {
        public Data.Entities.Task Task { get; set; }
        public int? SolutionId { get; set; }
        public string NewContent { get; set; }
        [Display(Name = "Załączniki")]
        public ICollection<SolutionFiles> Attachments { get; set; }
        public bool ValidForEdit()
        {
            return NewContent != null && NewContent.Length > 1 && SolutionId != null && SolutionId > 0;
        }
    }
}
