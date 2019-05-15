using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using archive.Data.Entities;

namespace archive.Models.Task
{
    public class TaskEditModel
    {
        public int Id { get; set; }

        public Data.Entities.Taskset Taskset { get; set; }
        public List<File> Attachments { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Nazwa zadania")]
        public string NewName { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Treść zadania")]
        public string NewContent { get; set; }
    }
}
