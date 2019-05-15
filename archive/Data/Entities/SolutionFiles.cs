using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace archive.Data.Entities
{
    public class SolutionFiles
    {
            public int SolutionId { get; set; }
            public virtual Solution Solution { get; set; }

            public Guid FileId { get; set; }
            public virtual File File { get; set; }
        }
}
