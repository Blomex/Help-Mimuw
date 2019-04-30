using System.ComponentModel.DataAnnotations;

namespace archive.Data.Entities
{
    public class UserAvatar
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(4096 * 4096 * 3)]
        public byte[] Image { get; set; }
        public string ApplicationUserId { get; set; }        
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}