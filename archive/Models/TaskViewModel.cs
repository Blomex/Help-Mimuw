namespace archive.Models
{
    public class TaskViewModel
    {
        public string Name { get; }
        public string Content { get; }

        public TaskViewModel(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}