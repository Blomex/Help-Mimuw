using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace archive.Models.Taskset
{
    public class AddAttachmentsModel
    {
        [HiddenInput]
        [Required]
        public int TasksetId { get; set; }
        public Data.Entities.Taskset Taskset { get; set; }
        
        [Display(Name = "Dodaj załączniki")]
        [Required]
        public List<IFormFile> Attachments { get; set; }
    }
}