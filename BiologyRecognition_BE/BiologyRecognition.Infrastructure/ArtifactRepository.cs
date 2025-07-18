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

        public IQueryable <Artifact> GetArtifactsByContainsNameAsync(string name)
        {
            return _context.Artifacts.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Include(c => c.ArtifactType).ThenInclude(at => at.Topic).Where(u => u.Name.ToLower().Contains(name.ToLower()));
        }
        public async Task<int> UpdateAsync(Artifact artifact)
        {
            artifact.CreatedByNavigation = null;
            artifact.ModifiedByNavigation = null;
            artifact.ArtifactType = null;
            var existing = await _context.Artifacts.FindAsync(artifact.ArtifactId);
            if (existing == null) return 0;

            _context.Artifacts.Update(artifact);

            return await _context.SaveChangesAsync();
        }
        public IQueryable<Artifact> GetAllAsync()
        {
            return  _context.Artifacts.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Include(c => c.ArtifactType).ThenInclude(at => at.Topic);
        }
        public async Task<Artifact> GetByIdAsync(int id)
        {
            return await _context.Artifacts.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Include(c => c.ArtifactType).ThenInclude(at => at.Topic).FirstOrDefaultAsync(h => h.ArtifactId == id);
        }
        public IQueryable<Artifact> GetListArtifactsByArtifactTypeIdAsync(int id)
        {
            return _context.Artifacts.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Include(c => c.ArtifactType).ThenInclude(at => at.Topic)
                .Where(u => u.ArtifactTypeId == id);
        }
        public IQueryable<Artifact> GetListArtifactsByListIdsAsync(List<int> artifactIds)
        {
            return  _context.Artifacts
                .Where(a => artifactIds.Contains(a.ArtifactId));
        }
    }
}
