using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using archive.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;


namespace archive.Data.Entities
{
    public class Taskset
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR(8)")]
        public string ShortcutCode { get; set; }
        public TasksetType Type { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        
        public int CourseId { get; set; }    
        public virtual Course Course { get; set; }
        
        public virtual ICollection<Task> Tasks { get; set; } = new HashSet<Task>();
        public virtual ICollection<File> Attachments { get; set; } = new HashSet<File>();
    }
}