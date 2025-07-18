using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface IArtifactService
    {
        Task<int> CreateAsync(Artifact artifact);
        Task<int> UpdateAsync(Artifact artifact);
        Task<List<Artifact>> GetAllAsync();
        Task<PagedResult<Artifact>> GetAllAsync(int page,int pageSize);
        Task<List<Artifact>> GetArtifactsByContainsNameAsync(string name);
        Task<PagedResult<Artifact>> GetArtifactsByContainsNameAsync(string name, int page, int pageSize);
        Task<Artifact> GetByIdAsync(int id);
        Task<List<Artifact>> GetListArtifactsByArtifactTypeIdAsync(int id);
        Task<PagedResult<Artifact>> GetListArtifactsByArtifactTypeIdAsync(int id, int page, int pageSize);
        Task<List<Artifact>> GetListArtifactsByListIdsAsync(List<int> artifactIds);
        Task<PagedResult<Artifact>> GetListArtifactsByListIdsAsync(List<int> artifactIds, int page, int pageSize);
    }
}
