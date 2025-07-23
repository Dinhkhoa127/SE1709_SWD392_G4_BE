using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface IArtifactTypeService
    {
        Task<int> CreateAsync(ArtifactType artifactType);
        Task<int> UpdateAsync(ArtifactType artifactType);
        Task<List<ArtifactType>> GetAllAsync();
        Task<PagedResult<ArtifactType>> GetAllAsync(int page, int pageSize);
        Task<List<ArtifactType>> GetArtifactTypesByContainsNameAsync(string name);
        Task<PagedResult<ArtifactType>> GetArtifactTypesByContainsNameAsync(string name, int page, int pageSize);
        Task<ArtifactType> GetByIdAsync(int id);
        Task<List<ArtifactType>> GetListArtifactTypesByTopicIdAsync(int id);
        Task<PagedResult<ArtifactType>> GetListArtifactTypesByTopicIdAsync(int id, int page, int pageSize);
    }
}
