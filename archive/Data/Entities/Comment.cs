using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace archive.Data.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string author { get; set; }
        public string content { get; set; }
        public int SolutionId { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CommentDate { get; set; }
        public virtual Solution Solution { get; set; }
        
    }
}