using archive.Data.Entities;

namespace archive.Models.Solution
{
    public class SolutionViewModel
    {
        public Data.Entities.Task Task { get; }
        public Data.Entities.Solution Solution { get; }

        public SolutionViewModel(Data.Entities.Task task, Data.Entities.Solution solution)
        {
            Task = task;
            Solution = solution;
        }
    }
}
