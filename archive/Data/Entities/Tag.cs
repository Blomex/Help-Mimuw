using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace archive.Data.Entities
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        public int TakId { get; set; }

        public string Name { get; set; }

        public virtual Task Task { get; set; }
    }
}