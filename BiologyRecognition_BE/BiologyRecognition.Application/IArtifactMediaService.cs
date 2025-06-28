using BiologyRecognition.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public interface IArtifactMediaService
    {
        Task<int> CreateAsync(ArtifactMedia artifactMedia);
        Task<int> UpdateAsync(ArtifactMedia artifactMedia);
        Task<bool> DeleteAsync(ArtifactMedia artifactMedia);
        Task<List<ArtifactMedia>> GetAllAsync();
        Task<List<ArtifactMedia>> GetListArtifactMediaByTypeAsync(string type);
        Task<ArtifactMedia> GetByIdAsync(int id);
        Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactIdAsync(int id);
        Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactNameAsync(string name); 
    }
}
