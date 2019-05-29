using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using archive.Data.Entities;

namespace archive.Models.Task
{
    public class EditTagsViewModel
    {
        public Data.Entities.Task Task { get; set;}

        public int TaskId { get; set;}

        [Display(Name = "Tagi")]
        public string Tags { get; set; }

        public EditTagsViewModel(Data.Entities.Task task, string tags)
        {
            this.Task = task;
            this.Tags = tags;
        }
        public EditTagsViewModel(){}
    }
}
