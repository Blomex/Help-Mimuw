using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using archive.Data.Entities;

namespace archive.Services
{
    public interface ITasksetService
    {
        Task<List<Taskset>> FindForCourseAsync(string courseName);
    }
}