using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = archive.Data.Entities.Task;

namespace archive.Services
{
    public interface ITaskService
    {
        Task<List<TaskEntity>> FindForTasksetAsync(string courseName, string tasksetName, int year);
    }
}