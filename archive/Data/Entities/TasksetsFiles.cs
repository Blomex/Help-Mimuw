using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace archive.Data.Entities
{
    public class TasksetsFiles
    {
        public int TasksetId { get; set; }
        public virtual Taskset Taskset { get; set; }

        public Guid FileId { get; set; }
        public virtual File File { get; set; }
    }
}