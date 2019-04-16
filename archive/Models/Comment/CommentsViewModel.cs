using archive.Data.Entities;
using System.Collections.Generic;

namespace archive.Models.Comment
{
    public class CommentsViewModel
    {
        public Data.Entities.Solution Solution { get; }
        public List<Data.Entities.Comment> Comments { get; }

        public CommentsViewModel(List<Data.Entities.Comment> comments, Data.Entities.Solution solution)
        {
            Solution = solution;
            Comments = comments;
        }

    }
}