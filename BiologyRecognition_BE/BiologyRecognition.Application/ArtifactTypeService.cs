using BiologyRecognition.Domain.DBContext;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public class ArtifactTypeService : IArtifactTypeService
    {
        private readonly ArtifactTypeRepository _repository;
        public ArtifactTypeService() => _repository ??= new ArtifactTypeRepository();
        public Task<int> CreateAsync(ArtifactType artifactType)
        {
            return _repository.CreateAsync(artifactType);
        }

        public Task<List<ArtifactType>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<List<ArtifactType>> GetArtifactTypesByContainsNameAsync(string name)
        {
            return _repository.GetArtifactTypesByContainsNameAsync(name);
        }

        public Task<ArtifactType> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<ArtifactType>> GetListArtifactTypesByTopicIdAsync(int id)
        {
            return _repository.GetListArtifactTypesByTopicIdAsync(id);
        }

        public Task<int> UpdateAsync(ArtifactType artifactType)
        {
            return _repository.UpdateAsync(artifactType);   
        }
    }
}
