using archive.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Job = System.Threading.Tasks.Task;

namespace archive.Data
{
    public interface IRepository
    {
        DbSet<Course> Courses { get; }
        DbSet<Taskset> Tasksets { get; }
        DbSet<Task> Tasks { get; }
        DbSet<Solution> Solutions { get; }
        
        Job SaveChangesAsync();
    }
}