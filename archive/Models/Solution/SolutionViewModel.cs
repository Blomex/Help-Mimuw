using System.Collections.Generic;
using archive.Data.Entities;

namespace archive.Models.Solution
{
    public class SolutionViewModel
    {
        public Data.Entities.Task Task { get; }
        public Data.Entities.Solution Solution { get; }

        public int GoodVotes {get;}
        public int Counter { get; }

        public List<Data.Entities.Comment> Comments { get; }

        public SolutionViewModel(Data.Entities.Task task, Data.Entities.Solution solution, List<Data.Entities.Comment> comments, int good, int counter)
        {
            Task = task;
            Solution = solution;
            GoodVotes=good;
            Comments = comments;
            Counter = counter;
        }
    }
}
