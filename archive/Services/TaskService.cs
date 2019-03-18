using System.Collections.Generic;
using System.Linq;
using archive.Data;
using archive.Data.Entities;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.EntityFrameworkCore;

namespace archive.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository _repository;

        public TaskService(IRepository repository)
        {
            _repository = repository;
        }

        public System.Threading.Tasks.Task<List<Task>>
            FindForTasksetAsync(string courseName, string tasksetName, int year)
        {
            return _repository.Tasks
                .Where(t => t.Taskset.Year == year &&
                            t.Taskset.Name == tasksetName &&
                            t.Taskset.Course.Name == courseName)
                .ToListAsync();
        }
    }
}