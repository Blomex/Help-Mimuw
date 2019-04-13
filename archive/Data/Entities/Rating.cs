using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archive.Data.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public int IdSolution { get; set; }
        public int IdUser { get; set; }
        public int Value {get; set; }

    }
}