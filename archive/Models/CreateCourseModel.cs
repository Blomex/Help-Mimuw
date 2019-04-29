using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace archive.Models
{
    [Display(Name = "Dodaj przedmiot")]
    public class CreateCourseModel
    {
        [Display(Name = "Nazwa")]
        [Required] 
        public string Name { get; set; }

        [Display(Name = "Skrót")] 
        public string ShortcutCode { get; set; }

        public CreateCourseModel()
        {
        }

    }
}