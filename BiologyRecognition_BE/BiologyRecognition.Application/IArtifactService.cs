using BiologyRecognition.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public interface IArtifactService
    {
        Task<int> CreateAsync(Artifact artifact);
        Task<int> UpdateAsync(Artifact artifact);
        Task<List<Artifact>> GetAllAsync();
        Task<List<Artifact>> GetArtifactsByContainsNameAsync(string name);
        Task<Artifact> GetByIdAsync(int id);
        Task<List<Artifact>> GetListArtifactsByArtifactTypeIdAsync(int id);
        Task<List<Artifact>> GetListArtifactsByListIdsAsync(List<int> artifactIds);
    }
}
