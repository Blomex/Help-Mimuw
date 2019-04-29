using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archive.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(4096 * 4096 * 3)]
        public byte[] Avatar { get; set; }
        [MaxLength(256)]
        public string HomePage { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime LastLogout { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; }
    }
}