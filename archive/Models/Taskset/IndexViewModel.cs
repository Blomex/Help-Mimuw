using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace archive.Models.Taskset
{
    public class IndexViewModel
    {
        public Data.Entities.Course Course { get; set; }
        public IEnumerable<Data.Entities.Taskset> Tasksets { get; set;}

        [Display(Name = "Czy posiada zadania?")]
        public bool haveTasks {get; set;}
        [Display(Name = "Czy posiada rozwiązane zadania?")]
        public bool haveSolutions {get; set;}
        [Display(Name = "Egzaminy od:")]
        public int yearFrom {get; set;} = 2010;
        [Display(Name = "Egzaminy do:")]
        public int yearTo {get; set;} = 2020;

    }

}