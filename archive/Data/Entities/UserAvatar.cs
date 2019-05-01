using System.ComponentModel.DataAnnotations;

namespace archive.Data.Entities
{
    public class UserAvatar
    {
        public const int ImageSizeLimit = 1024 * 1024 * 3;
        
        [Key]
        public int Id { get; set; }
        [MaxLength(ImageSizeLimit)]
        public byte[] Image { get; set; }
        public string ApplicationUserId { get; set; }        
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}