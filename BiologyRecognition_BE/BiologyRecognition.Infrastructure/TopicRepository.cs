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
    public class TopicRepository : GenericRepository<Topic>
    {
        public TopicRepository() { }
        public TopicRepository(SE1709_SWD392_G4_BiologyRecognitionSystemContext context) => _context = context;

        public async Task<List<Topic>> GetTopicsByContainsNameAsync(string name)
        {
            return await _context.Topics.Include(c => c.Chapter).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Where(u => u.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        public async Task<int> UpdateAsync(Topic topic)
        {
            topic.ModifiedByNavigation = null;
            topic.Chapter = null;
            var existing = await _context.Chapters.FindAsync(topic.TopicId);
            if (existing == null) return 0;

            _context.Topics.Update(topic);

            return await _context.SaveChangesAsync();
        }
        public async Task<List<Topic>> GetAllAsync()
        {
            return await _context.Topics.Include(c => c.Chapter).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).ToListAsync();
        }
        public async Task<Topic> GetByIdAsync(int id)
        {
            return await _context.Topics.Include(c => c.Chapter).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).FirstOrDefaultAsync(h => h.TopicId == id);
        }
        public async Task<List<Topic>> GetListTopicsByChapterIdAsync(int id)
        {
            return await _context.Topics.Include(c => c.Chapter).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation)
                .Where(u => u.ChapterId == id)
                .ToListAsync();
        }

        public async Task<List<Topic>> GetListTopicsByArtifactNameAsync(string artifactName)
        {
            return await _context.Topics.Include(a => a.ArtifactTypes).ThenInclude(a => a.Artifacts).Where(a => a.ArtifactTypes.Any(a => a.Artifacts.Any(ar => ar.Name.ToLower().Contains(artifactName)))).ToListAsync();
        }

    }
}
