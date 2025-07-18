using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface IArtifactMediaService
    {
        Task<int> CreateAsync(ArtifactMedia artifactMedia);
        Task<int> UpdateAsync(ArtifactMedia artifactMedia);
        Task<bool> DeleteAsync(ArtifactMedia artifactMedia);
        Task<PagedResult<ArtifactMedia>> GetAllAsync(int page, int pageSize);
        Task<List<ArtifactMedia>> GetAllAsync();
        Task<PagedResult<ArtifactMedia>> GetListArtifactMediaByTypeAsync(string type, int page, int pageSize);
        Task<List<ArtifactMedia>> GetListArtifactMediaByTypeAsync(string type);
        Task<ArtifactMedia> GetByIdAsync(int id);
        Task<PagedResult<ArtifactMedia>> GetListArtifactMediaByArtifactIdAsync(int id, int page, int pageSize);
        Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactIdAsync(int id);
        Task<PagedResult<ArtifactMedia>> GetListArtifactMediaByArtifactNameAsync(string name, int page, int pageSize);
        Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactNameAsync(string name);
    }
}
