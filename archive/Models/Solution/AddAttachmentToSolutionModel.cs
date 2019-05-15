using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace archive.Models.Solution
{
    
        public class AddAttachmentToSolutionModel
        {
            [HiddenInput]
            [Required]
            public int SolutionId { get; set; }
            public Data.Entities.Solution Solution { get; set; }

            [Display(Name = "Dodaj załączniki")]
            [Required]
            public List<IFormFile> Attachments { get; set; }
        }
    
}
