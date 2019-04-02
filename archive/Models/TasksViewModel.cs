using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace archive.Models
{
    public class TasksViewModel
    {
        public IEnumerable<TaskViewModel> Tasks { get; }
        public string TasksetName { get; }
        public string CourseName { get; }
        public int TasksetYear { get; }

        public TasksViewModel(IEnumerable<TaskViewModel> tasks,
            string tasksetName, string courseName, int tasksetYear)
        {
            Tasks = tasks;
            TasksetName = tasksetName;
            CourseName = courseName;
            TasksetYear = tasksetYear;
        }
    }
}