using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace archive.Data.Entities
{
    public class File
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(127)")]
        public string MimeType { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(127)")]
        public string MimeSubtype { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(256)")]
        public string FileName { get; set; }

        [NotMapped]
        public string Path { get; set; }
        
        public virtual ICollection<TasksetsFiles> TasksetReferers { get; set; } = new HashSet<TasksetsFiles>();
        public virtual ICollection<TasksFiles> TasksReferers { get; set; } = new HashSet<TasksFiles>();
    }
}
