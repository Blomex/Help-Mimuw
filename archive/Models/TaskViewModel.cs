using System.Collections.Generic;
using archive.Data.Entities;

namespace archive.Models
{
    public class TaskViewModel
    {
        public int Id { get; }
        public string Name { get; }
        public string Content { get; }
        public IEnumerable<Solution> Solutions { get; }

        public TaskViewModel(int id, string name, string content, IEnumerable<Solution> solutions)
        {
            Name = name;
            Content = content;
            Solutions = solutions;
            Id = id;
        }
    }
}