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
    public class ArtifactTypeRepository : GenericRepository<ArtifactType>
    {
        public ArtifactTypeRepository() { }
        public ArtifactTypeRepository(SE1709_SWD392_G4_BiologyRecognitionSystemContext context) => _context = context;

        public async Task<List<ArtifactType>> GetArtifactTypesByContainsNameAsync(string name)
        {
            return await _context.ArtifactTypes.Include(c => c.Topic).Where(u => u.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        public async Task<int> UpdateAsync(ArtifactType artifactType)
        {

            artifactType.Topic = null;
            var existing = await _context.Chapters.FindAsync(artifactType.TopicId);
            if (existing == null) return 0;

            _context.ArtifactTypes.Update(artifactType);

            return await _context.SaveChangesAsync();
        }
        public async Task<List<ArtifactType>> GetAllAsync()
        {
            return await _context.ArtifactTypes.Include(c => c.Topic).ToListAsync();
        }
        public async Task<ArtifactType> GetByIdAsync(int id)
        {
            return await _context.ArtifactTypes.Include(c => c.Topic).FirstOrDefaultAsync(h => h.ArtifactTypeId == id);
        }
        public async Task<List<ArtifactType>> GetListArtifactTypesByTopicIdAsync(int id)
        {
            return await _context.ArtifactTypes.Include(c => c.Topic)
                .Where(u => u.TopicId == id)
                .ToListAsync();
        }

    }
}
