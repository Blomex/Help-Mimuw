using archive.Data.Entities;
using System.Collections.Generic;

namespace archive.Models
{
    public class SolutionViewModel
    {
        public Task Task { get; }
        public Solution Solution { get; }

        public SolutionViewModel(Task task, Solution solution)
        {
            Task = task;
            Solution = solution;
        }
    }
}
