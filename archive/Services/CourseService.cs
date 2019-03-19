using System.Collections.Generic;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace archive.Services
{
    public class CourseService : ICourseService
    {
        private readonly IRepository _repository;

        public CourseService(IRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Course>> FindAllAsync()
        {
            return _repository.Courses.ToListAsync();
        }
    }
}