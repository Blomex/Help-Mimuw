using System.Collections.Generic;

namespace archive.Models.Taskset
{
    public class IndexViewModel
    {
        public Data.Entities.Course Course { get; set; }
        public IEnumerable<Data.Entities.Taskset> Tasksets { get; set;}
    }

}