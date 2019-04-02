using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace archive.Models.Task
{
    [Display(Name = "Dodaj zadanie")]
    public class CreateTaskViewModel
    {
        [Display(Name = "Nazwa")]
        [Required] 
        public string Name { get; set; }

        [Display(Name = "Treść")] 
        public string Content { get; set; }

        [Display(Name = "Zbiór zadań")]
        [Required]
        public int TasksetId { get; set; }

        public List<SelectListItem> Tasksets { get; }

        [Display(Name = "Przedmiot")]
        public List<SelectListItem> Courses { get; }

        public CreateTaskViewModel()
        {
        }

        public CreateTaskViewModel(List<Data.Entities.Taskset> tasksets)
        {
            var coursesNames = tasksets
                .Select(t => t.Course.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToList();

            Courses = coursesNames
                .Select(n => new SelectListItem(n, n))
                .ToList();

            var courseGroups = coursesNames
                .Select(n => new SelectListGroup {Name = n})
                .ToDictionary(g => g.Name);

            Tasksets = tasksets
                .OrderBy(t => t.Course.Name)
                .ThenByDescending(t => t.Year)
                .ThenBy(t => t.Name)
                .Select(t => new SelectListItem
                {
                    Text = $"({t.Year}) {t.Name}", Value = t.Id.ToString(),
                    Group = courseGroups[t.Course.Name]
                })
                .ToList();
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Content)}: {Content}, {nameof(TasksetId)}: {TasksetId}";
        }
    }
}