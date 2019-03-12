using System.Collections.Generic;

namespace archive.Data.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Taskset> Tasksets { get; set; } = new HashSet<Taskset>();
    }
}