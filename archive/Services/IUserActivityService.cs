using System;
using System.Threading.Tasks;

namespace archive.Services
{
    public interface IUserActivityService
    {
        Task RegisterActionAsync(string name);
        Task<DateTime?> GetLastActionTimeAsync(string name);
    }
}