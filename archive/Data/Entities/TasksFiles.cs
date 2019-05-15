using System;

namespace archive.Data.Entities
{
    public class TasksFiles
    {
        public int TaskId { get; set; }
        public virtual Task Task { get; set; }

        public Guid FileId { get; set; }
        public virtual File File { get; set; }
    }
}