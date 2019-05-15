using System;

namespace archive.Data.Entities
{
    public class SolutionsFiles
    {
        public int SolutionId { get; set; }
        public virtual Solution Solution { get; set; }

        public Guid FileId { get; set; }
        public virtual File File { get; set; }
    }
}