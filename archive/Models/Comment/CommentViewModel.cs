using archive.Data.Entities;
using System.Collections.Generic;

namespace archive.Models.Comment
{
    public class CommentViewModel
    {
        public Data.Entities.Solution Solution { get; set; }
        public Data.Entities.Comment Comment { get; set; }

        public CommentViewModel(Data.Entities.Comment comment, Data.Entities.Solution solution)
        {
            Solution = solution;
            Comment = comment;
        }

    }
}