using System.Collections.Generic;
using System.Threading.Tasks;
using archive.Data.Entities;

namespace archive.Services
{
    public interface IRatingService
    {
        Task<List<Rating>> FindAllAsync();
    }
}