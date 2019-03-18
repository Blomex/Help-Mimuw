using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace archive.Data.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Taskset> Tasksets { get; set; } = new HashSet<Taskset>();
    }
}