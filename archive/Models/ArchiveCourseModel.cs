using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace archive.Models
{
    [Display(Name = "Archwizuj przedmiot")]
    public class ArchiveCourseModel
    {
        public List<SelectListItem> Courses { get; }

        [Display(Name = "Wybierz przedmiot")]
        [Required]
        public int CourseId { get; set; }

        public ArchiveCourseModel(List<Data.Entities.Course> courses)
        {
            Courses = courses
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToList();
        }

        public ArchiveCourseModel(){}

    }
}