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
    public class ArtifactMediaRepository:GenericRepository<ArtifactMedia>
    {
        public ArtifactMediaRepository() { }
        public ArtifactMediaRepository(SE1709_SWD392_G4_BiologyRecognitionSystemContext context) => _context = context;

    
        public async Task<List<ArtifactMedia>> GetListArtifactMediaByTypeAsync(string type)
        {
            return await _context.ArtifactMedia.Include(c => c.Artifact)
                .Where(u => u.Type.ToLower().Contains(type.ToLower()))
                .ToListAsync();
        }
        public async Task<int> UpdateAsync(ArtifactMedia artifactMedia)
        {
            artifactMedia.Artifact = null;
           
            var existing = await _context.Chapters.FindAsync(artifactMedia.ArtifactMediaId);
            if (existing == null) return 0;

            _context.ArtifactMedia.Update(artifactMedia);

            return await _context.SaveChangesAsync();
        }
        public async Task<List<ArtifactMedia>> GetAllAsync()
        {
            return await _context.ArtifactMedia.Include(c => c.Artifact).ToListAsync();
        }

        public async Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactIdAsync(int id)
        {
            return await _context.ArtifactMedia.Include(c => c.Artifact)
                .Where(u => u.ArtifactId == id)
            .ToListAsync();
        }

        public async Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactNameAsync(string name)
        {
            return await _context.ArtifactMedia.Include(c => c.Artifact)
                .Where(u => u.Artifact.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }
        public async Task<ArtifactMedia> GetByIdAsync(int id)
        {
            return await _context.ArtifactMedia.Include(c => c.Artifact).FirstOrDefaultAsync(h => h.ArtifactMediaId == id);
        }

    }
}
