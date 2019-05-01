using System.ComponentModel.DataAnnotations;

namespace archive.Data.Entities
{
    public class UserAvatar
    {
        public const int ImageSizeLimit = 1024 * 1024 * 3;
        
        [MaxLength(ImageSizeLimit)]
        public byte[] Image { get; set; }
        [Key]
        public string ApplicationUserId { get; set; }        
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}