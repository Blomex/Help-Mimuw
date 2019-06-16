using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace archive.Data.Entities
{
    public class UsersAchievements
    {
        public virtual string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual int AchievementId { get; set; }
        public virtual Achievement Achievement { get; set; }
    }
}
