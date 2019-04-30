using System;
using System.Threading.Tasks;

namespace archive.Services
{
    public interface IUserActivityService
    {
        void RegisterAction(string name);
        Task<Nullable<DateTime>> GetLastActionTimeAsync(string name);
    }
}