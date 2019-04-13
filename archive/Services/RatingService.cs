using System.Collections.Generic;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace archive.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRepository _repository;

        public RatingService(IRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Rating>> FindAllAsync()
        {
            return _repository.Ratings.ToListAsync();
        }
    }
}