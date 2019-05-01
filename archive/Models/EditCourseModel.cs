using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace archive.Models
{
    [Display(Name = "Edytuj nazwę przedmiotu")]
    public class EditCourseModel
    {
        public List<SelectListItem> Courses { get; }

        [Display(Name = "Przedmiot")]
        [Required]
        public int CourseId { get; set; }

        [Display(Name = "Nowa nazwa")]
        [Required] 
        public string Name { get; set; }

        public EditCourseModel(List<Data.Entities.Course> courses)
        {
            Courses = courses
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToList();
        }

        public EditCourseModel(){}

    }
}