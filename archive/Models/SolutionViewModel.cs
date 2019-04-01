namespace archive.Models
{
    public class SolutionViewModel
    {
        public string Content { get; }

        public SolutionViewModel(string content)
        {
            Content = content;
        }
    }
}