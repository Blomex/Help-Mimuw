using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archive.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(256)]
        public string HomePage { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime LastActive { get; set; }

        public virtual UserAvatar Avatar { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Solution> Solutions { get; set; }

        public virtual ICollection<UsersAchievements> UsersAchievements { get; set; }
    }
}