using System.Collections.Generic;

namespace archive.Models
{
    public class TasksetsViewModel
    {
        public IEnumerable<TasksetViewModel> Tasksets { get; }
        public string CourseName { get; }

        public TasksetsViewModel(IEnumerable<TasksetViewModel> tasksets, string courseName)
        {
            Tasksets = tasksets;
            CourseName = courseName;
        }
    }
}
