using System.Collections.Generic;

namespace archive.Models
{
    public class SolutionsViewModel
    {
        public string TaskName { get; }
        public IEnumerable<SolutionViewModel> Solutions { get; }
        public string TasksetName { get; }
        public string CourseName { get; }

        public int TasksetYear { get; }

        public SolutionsViewModel(IEnumerable<SolutionViewModel> solutions, string taskName, string tasksetName, string courseName, int tasksetYear)
        {
            Solutions = solutions;
            TaskName = taskName;
            TasksetName = tasksetName;
            CourseName = courseName;
            TasksetYear = tasksetYear;

        }

    }
}