using BiologyRecognition.Domain.Entities;
using BiologyRecognition.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public class ArtifactMediaService : IArtifactMediaService
    {
        private readonly ArtifactMediaRepository _repository;
        public ArtifactMediaService() => _repository ??= new ArtifactMediaRepository();
        public Task<int> CreateAsync(ArtifactMedia artifactMedia)
        {
            return _repository.CreateAsync(artifactMedia);
        }

        public Task<bool> DeleteAsync(ArtifactMedia artifactMedia)
        {
            return _repository.RemoveAsync(artifactMedia);
        }

        public Task<List<ArtifactMedia>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<ArtifactMedia> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactIdAsync(int id)
        {
            return _repository.GetListArtifactMediaByArtifactIdAsync(id);
        }

        public Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactNameAsync(string name)
        {
            return _repository.GetListArtifactMediaByArtifactNameAsync(name);
        }

        public Task<List<ArtifactMedia>> GetListArtifactMediaByTypeAsync(string type)
        {
            return _repository.GetListArtifactMediaByTypeAsync(type);
        }

        public Task<int> UpdateAsync(ArtifactMedia artifactMedia)
        {
            return _repository.UpdateAsync(artifactMedia);
        }
    }
}
