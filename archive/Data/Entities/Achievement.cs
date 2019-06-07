using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace archive.Data.Entities
{
    [FlagsAttribute]
    public enum AchievementFlags : short
    {
        None = 0,
        HiddenDescription = 1
    }

    public class Achievement
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        public string NormalizedName { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(40)")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR(120)")]
        public string IconPath { get; set; }

        public AchievementFlags AchievementFlags { get; set; }

        [Required]
        public string Description { get; set; }


        public virtual ICollection<UsersAchievements> UsersAchievements { get; set; }
    }
}
