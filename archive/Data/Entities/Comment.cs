using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace archive.Data.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string content { get; set; }
        public int SolutionId { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CommentDate { get; set; }
        public virtual Solution Solution { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}