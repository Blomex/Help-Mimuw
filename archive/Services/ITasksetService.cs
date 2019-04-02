using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using archive.Data.Entities;
using Task = System.Threading.Tasks.Task;

namespace archive.Services
{
    public interface ITasksetService
    {
        Task<List<Taskset>> FindForCourseAsync(string courseName);

        Task AddTasksetAsync(Taskset taskset);
    }
}