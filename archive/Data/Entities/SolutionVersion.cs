using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace archive.Data.Entities
{
    public class SolutionVersion
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public virtual Solution Solution { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        [MinLength(20)]
        public string Content { get; set; }
    }
}
