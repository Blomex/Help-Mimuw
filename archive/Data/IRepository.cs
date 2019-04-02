using archive.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace archive.Data
{
    public interface IRepository
    {
        DbSet<Course> Courses { get; }
        DbSet<Taskset> Tasksets { get; }
        DbSet<Task> Tasks { get; }
        DbSet<Solution> Solutions { get; }
        
        System.Threading.Tasks.Task SaveChangesAsync();
    }
}