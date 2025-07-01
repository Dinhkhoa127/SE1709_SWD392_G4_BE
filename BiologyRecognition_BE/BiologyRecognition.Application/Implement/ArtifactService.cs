using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Implement
{
    public class ArtifactService : IArtifactService
    {

        private readonly ArtifactRepository _artifactRepository;
        public ArtifactService() => _artifactRepository ??= new ArtifactRepository();

        public Task<int> CreateAsync(Artifact artifact)
        {
            return _artifactRepository.CreateAsync(artifact);
        }

        public Task<List<Artifact>> GetAllAsync()
        {
            return _artifactRepository.GetAllAsync();
        }

        public Task<List<Artifact>> GetArtifactsByContainsNameAsync(string name)
        {
            return _artifactRepository.GetArtifactsByContainsNameAsync(name);
        }

        public Task<Artifact> GetByIdAsync(int id)
        {
            return _artifactRepository.GetByIdAsync(id);
        }

        public Task<List<Artifact>> GetListArtifactsByArtifactTypeIdAsync(int id)
        {
            return _artifactRepository.GetListArtifactsByArtifactTypeIdAsync(id);
        }

        public Task<int> UpdateAsync(Artifact artifact)
        {
            return _artifactRepository.UpdateAsync(artifact);
        }
        public Task<List<Artifact>> GetListArtifactsByListIdsAsync(List<int> artifactIds)
        {
            return _artifactRepository.GetListArtifactsByListIdsAsync(artifactIds);
        }
    }
}
