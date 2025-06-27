using BiologyRecognition.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public interface IArtifactTypeService
    {
        Task<int> CreateAsync(ArtifactType artifactType);
        Task<int> UpdateAsync(ArtifactType artifactType);
        Task<List<ArtifactType>> GetAllAsync();
        Task<List<ArtifactType>> GetArtifactTypesByContainsNameAsync(string name);
        Task<ArtifactType> GetByIdAsync(int id);
        Task<List<ArtifactType>> GetListArtifactTypesByTopicIdAsync(int id);
    }
}
