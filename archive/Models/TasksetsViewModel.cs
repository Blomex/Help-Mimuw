using System.Collections.Generic;

namespace archive.Models
{
    public class TasksetsViewModel
    {
        public IEnumerable<TasksetViewModel> Tasksets { get; }
        public string CourseName { get; }
        public int Year { get; }

        public TasksetsViewModel(IEnumerable<TasksetViewModel> tasksets, string courseName, int year)
        {
            Tasksets = tasksets;
            CourseName = courseName;
            Year = year;
        }
    }
}
