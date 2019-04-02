using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using System.Linq;
using SolutionEntity = archive.Data.Entities.Solution;
using Microsoft.EntityFrameworkCore;

namespace archive.Services
{
    public class SolutionService : ISolutionService
    {

        private readonly IRepository _repository;

        public SolutionService(IRepository repository)
        {
            _repository = repository;
        }
        public Task<List<SolutionEntity>> FindForTaskAsync(string taskName, string tasksetName, string courseName,
            int year)
        {
            return _repository.Solutions
                .Where(t => t.Task.Taskset.Year == year &&
                            t.Task.Taskset.Name == tasksetName &&
                            t.Task.Taskset.Course.Name == courseName &&
                            t.Task.Name == taskName)
                .ToListAsync();
        }

    }
}