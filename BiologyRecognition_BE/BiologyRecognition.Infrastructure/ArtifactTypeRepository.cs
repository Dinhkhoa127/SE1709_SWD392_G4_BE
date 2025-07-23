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

        public IQueryable<ArtifactType> GetArtifactTypesByContainsNameAsync(string name)
        {
            return _context.ArtifactTypes.Include(c => c.Topic).Where(u => u.Name.ToLower().Contains(name.ToLower()));
        }
        public async Task<int> UpdateAsync(ArtifactType artifactType)
        {

            artifactType.Topic = null;
            var existing = await _context.ArtifactTypes.FindAsync(artifactType.TopicId);
            if (existing == null) return 0;

            _context.ArtifactTypes.Update(artifactType);

            return await _context.SaveChangesAsync();
        }

        public IQueryable<ArtifactType> GetAllAsync()
        {
            return _context.ArtifactTypes.Include(c => c.Topic);
        }
        public async Task<ArtifactType> GetByIdAsync(int id)
        {
            return await _context.ArtifactTypes.Include(c => c.Topic).FirstOrDefaultAsync(h => h.ArtifactTypeId == id);
        }

        public IQueryable<ArtifactType> GetListArtifactTypesByTopicIdAsync(int id)
        {
            return _context.ArtifactTypes.Include(c => c.Topic)
                .Where(u => u.TopicId == id);
        }


    }
}
