namespace archive.Models
{
    public class CourseViewModel
    {
        public string Name { get; }

        public int Id {get;}

        public CourseViewModel(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}