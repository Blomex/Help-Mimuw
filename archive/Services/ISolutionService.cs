using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SolutionEntity = archive.Data.Entities.Solution;

namespace archive.Services
{
    public interface ISolutionService
    {
        Task<List<SolutionEntity>> FindForTaskAsync(string taskName, string tasksetName, string courseName, int tasksetYear);

    }
}