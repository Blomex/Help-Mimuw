using System.Collections.Generic;


namespace archive.Models.Taskset
{
    public class TasksetViewModel
    {
        public Data.Entities.Taskset Taskset { get; set; }
        public IEnumerable<Data.Entities.Task> Tasks { get; set; }

        public Dictionary<int, List<Data.Entities.Solution>> ListOfSolutions{ get; set;}
    }
}