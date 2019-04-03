using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace archive.Services
{
    public class TasksetService : ITasksetService
    {
        private readonly IRepository _repository;

        public TasksetService(IRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Taskset>> FindForCourseAsync(string courseName)
        {
            return _repository.Tasksets
                .Where(t => t.Course.Name == courseName)
                .ToListAsync();
        }

        public Task AddTasksetAsync(Taskset taskset)
        {
            _repository.Tasksets.Add(taskset);
            return _repository.SaveChangesAsync();
        }
    }
}