using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archive.Data.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR(8)")]
        public string ShortcutCode { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Taskset> Tasksets { get; set; } = new HashSet<Taskset>();
    }
}