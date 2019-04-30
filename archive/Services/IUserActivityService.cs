using System;
using System.Threading.Tasks;

namespace archive.Services
{
    public interface IUserActivityService
    {
        void RegisterAction(string uid);
        Task<Nullable<DateTime>> GetLastActionTimeAsync(string uid);
    }
}