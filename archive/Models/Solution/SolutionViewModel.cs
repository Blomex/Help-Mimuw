using System.Collections.Generic;
using archive.Data.Entities;

namespace archive.Models.Solution
{
    public class SolutionViewModel
    {
        public Data.Entities.Task Task { get; }
        public Data.Entities.Solution Solution { get; }

        public double Average {get;}

        public List<Data.Entities.Comment> Comments { get; }

        public SolutionViewModel(Data.Entities.Task task, Data.Entities.Solution solution, List<Data.Entities.Comment> comments, double average)
        {
            Task = task;
            Solution = solution;
            Average=average;
            Comments = comments;
        }
    }
}
