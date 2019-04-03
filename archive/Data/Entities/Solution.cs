using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archive.Data.Entities
{
    public class Solution
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        
        public int TaskId { get; set; }
        
        public virtual Task Task { get; set; }
    }
}