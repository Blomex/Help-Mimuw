using archive.Data.Entities;
using archive.Data.Enums;

namespace archive.Models
{
    public class TasksetViewModel
    {
        public TasksetType Type { get; }
        public int Year { get; }
        public string Name { get; }

        public TasksetViewModel(TasksetType type, int year, string name)
        {
            Type = type;
            Year = year;
            Name = name;
        }
    }
}