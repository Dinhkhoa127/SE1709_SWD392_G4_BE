using BiologyRecognition.Domain.DBContext;
using BiologyRecognition.Domain.Entities;
using BloodDonation.Repositories.NhanNB.Basic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Infrastructure
{
    public class ArtifactRepository:GenericRepository<Artifact>
    {
        public ArtifactRepository() { }
        public ArtifactRepository(SE1709_SWD392_G4_BiologyRecognitionSystemContext context) => _context = context;

        public async Task<List<Artifact>> GetArtifactsByContainsNameAsync(string name)
        {
            return await _context.Artifacts.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Include(c => c.ArtifactType).ThenInclude(at => at.Topic).Where(u => u.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
        public async Task<int> UpdateAsync(Artifact artifact)
        {
            artifact.CreatedByNavigation = null;
            artifact.ModifiedByNavigation = null;
            artifact.ArtifactType = null;
            var existing = await _context.Chapters.FindAsync(artifact.ArtifactId);
            if (existing == null) return 0;

            _context.Artifacts.Update(artifact);

            return await _context.SaveChangesAsync();
        }
        public async Task<List<Artifact>> GetAllAsync()
        {
            return await _context.Artifacts.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Include(c => c.ArtifactType).ThenInclude(at => at.Topic).ToListAsync();
        }
        public async Task<Artifact> GetByIdAsync(int id)
        {
            return await _context.Artifacts.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Include(c => c.ArtifactType).ThenInclude(at => at.Topic).FirstOrDefaultAsync(h => h.ArtifactId == id);
        }
        public async Task<List<Artifact>> GetListArtifactsByArtifactTypeIdAsync(int id)
        {
            return await _context.Artifacts.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Include(c => c.ArtifactType).ThenInclude(at => at.Topic)
                .Where(u => u.ArtifactTypeId == id)
                .ToListAsync();
        }
        public async Task<List<Artifact>> GetListArtifactsByListIdsAsync(List<int> artifactIds)
        {
            return await _context.Artifacts
                .Where(a => artifactIds.Contains(a.ArtifactId))
                .ToListAsync();
        }
    }
}
