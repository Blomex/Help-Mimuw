using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using archive.Data.Entities;
using archive.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace archive.Models
{
    [Display(Name = "Utwórz zbiór zadań.")]
    public class CreateTasksetViewModel
    {
        [Display(Name = "Typ")] [Required] public string Type { get; set; }

        [Display(Name = "Rok")] [Required] public int Year { get; set; }

        [Display(Name = "Nazwa")] [Required] public string Name { get; set; }

        [Display(Name = "Przedmiot")]
        [Required]
        public int CourseId { get; set; }

        public List<SelectListItem> Types { get; }
        public TasksetType TypeAsEnum => Enum.Parse<TasksetType>(Type);

        public List<SelectListItem> Courses { get; }

        public CreateTasksetViewModel()
        {
        }

        public CreateTasksetViewModel(List<Course> courses)
        {
            Types = Enum.GetNames(typeof(TasksetType))
                .Select(t => new SelectListItem(t, t))
                .ToList();
            Courses = courses
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToList();
        }
    }
}