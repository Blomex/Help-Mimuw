using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using archive.Data.Entities;
using archive.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace archive.Models.Taskset
{
    [Display(Name = "Utwórz zbiór zadań.")]
    public class CreateTasksetViewModel
    {
        [Display(Name = "Typ")]
        [Required] 
        public string Type { get; set; }

        [Display(Name = "Rok")]
        [Required] 
        public int Year { get; set; }

        [Display(Name = "Nazwa")]
        [Required] 
        public string Name { get; set; }

        [Display(Name = "Przedmiot")]
        [Required]
        public int CourseId { get; set; }

        public List<SelectListItem> Types { get; }

        public TasksetType TypeAsEnum => Enum.Parse<TasksetType>(Type);

        public Data.Entities.Course Course { get; }

        public List<SelectListItem> Courses { get; }

        public CreateTasksetViewModel()
        {
            var translation = new Dictionary<string, string>();
            translation[TasksetType.Exam.ToString()] = "Egzamin";
            translation[TasksetType.Quiz.ToString()] = "Kartkówka";
            translation[TasksetType.Test.ToString()] = "Kolokwium";
            translation[TasksetType.Other.ToString()] = "Inny";
            
            Types = Enum.GetNames(typeof(TasksetType))
                .Select(t => new SelectListItem(translation[t], t))
                .ToList();
        }

        public CreateTasksetViewModel(Data.Entities.Course course) : this()
        {
            Course = course;
        }

        public CreateTasksetViewModel(List<Data.Entities.Course> courses) : this()
        {
            Courses = courses
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToList();
        }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(Year)}: {Year}, {nameof(Name)}: {Name}, {nameof(CourseId)}: {CourseId}";
        }
    }
}