using System.Collections.Generic;
using System.Threading.Tasks;
using archive.Data.Entities;

namespace archive.Services
{
    public interface ICourseService
    {
        Task<List<Course>> FindAllAsync();
    }
}