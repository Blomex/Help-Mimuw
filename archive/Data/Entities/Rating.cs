using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archive.Data.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public int IdSolution { get; set; }
        public string NameUser { get; set; }
        public bool Value {get; set; }

    }
}