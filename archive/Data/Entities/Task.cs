using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace archive.Data.Entities
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        /* Tu gienieczko ma listę wersji zamiast pojedyńczej zawartości. */
        public string Content { get; set; }
        public short? InTasksetNumber { get; set; }
   
        public int TasksetId { get; set; }
        public virtual Taskset Taskset { get; set; }
        public virtual ICollection<Solution> Solutions { get; set; } = new HashSet<Solution>();
    }
}