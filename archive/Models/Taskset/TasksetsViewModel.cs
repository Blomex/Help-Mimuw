using System.Collections.Generic;

namespace archive.Models.Taskset
{
    public class TasksetsViewModel
    {
        public IEnumerable<Data.Entities.Taskset> Tasksets { get; }
        public string CourseName { get; }

        public TasksetsViewModel(IEnumerable<Data.Entities.Taskset> tasksets, string courseName)
        {
            Tasksets = tasksets;
            CourseName = courseName;
        }
    }
}
