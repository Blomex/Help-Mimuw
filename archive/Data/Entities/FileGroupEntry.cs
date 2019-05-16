using System;
using System.Collections.Generic;

namespace archive.Data.Entities
{
    public class FileGroupEntry
    {
        public Guid FileGroupId { get; set; }

        public Guid FileId { get; set; }

        public virtual File File { get; set; }
    }
}
