using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace archive.Models.Taskset
{
    public class AllFilterTasksViewModel
    {
        public IEnumerable<Data.Entities.Task> Tasks { get; set;}

        public Dictionary<int, List<Data.Entities.Solution>> ListOfSolutions { get; set;}

        [Display(Name = "Czy posiada rozwiązane zadania?")]
        public bool haveSolutions {get; set;}
        [Display(Name = "Egzaminy od:")]
        public int yearFrom {get; set;} = 2010;
        [Display(Name = "Egzaminy do:")]
        public int yearTo {get; set;} = DateTime.Now.Year;

        [Display(Name = "Minimalna ocena:")]
        public double minRating {get; set;} = 0;

        [Display(Name = "Minimalna liczba ocen:")]
        public int minRatingNumber {get; set;} = 0;

        [Display(Name = "Wybierz przedmiot:")]
        public int courseId {get; set;} = 0;

        [Display(Name = "Wyszukaj zadania o konkretnym tagu (lub tagach, oddzielając je przecinkiem i spacją)")]
        public string Tags { get; set; }

        [Display(Name="Zaznacz jeśli chcesz wyszukać tylko zadania, które zawierają wszystkie tagi." +
                      "W przeciwnym razie zostaną wyszukane zadania które zawierają przynajmniej jeden z tagów.")]
        public bool allTags { get; set; }

        public List<SelectListItem> Courses { get; set;}
 
        public void AddCourseList(List<Data.Entities.Course> courses)
        {
            Courses = courses
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToList();

            Courses.Insert(0, new SelectListItem("Dowolny", "0"));
        }

        public AllFilterTasksViewModel(){}

    }
}