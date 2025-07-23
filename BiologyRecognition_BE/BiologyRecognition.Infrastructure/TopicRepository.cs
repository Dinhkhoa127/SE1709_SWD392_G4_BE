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

        public IQueryable<Topic> GetTopicsByContainsNameAsync(string name)
        {
            return _context.Topics.Include(c => c.Chapter).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).Where(u => u.Name.ToLower().Contains(name.ToLower()));
        }
        public async Task<int> UpdateAsync(Topic topic)
        {
            topic.ModifiedByNavigation = null;
            topic.Chapter = null;
            var existing = await _context.Topics.FindAsync(topic.TopicId);
            if (existing == null) return 0;

            _context.Topics.Update(topic);

            return await _context.SaveChangesAsync();
        }

        public IQueryable<Topic> GetAllAsync()
        {
            return _context.Topics.Include(c => c.Chapter).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation);
        }
        public async Task<Topic> GetByIdAsync(int id)
        {
            return await _context.Topics.Include(c => c.Chapter).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).FirstOrDefaultAsync(h => h.TopicId == id);
        }

        public IQueryable<Topic> GetListTopicsByChapterIdAsync(int id)
        {
            return  _context.Topics.Include(c => c.Chapter).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation)
                .Where(u => u.ChapterId == id);
        }
        
        public IQueryable<Topic> GetListTopicsByArtifactNameAsync(string artifactName)
        {
            return  _context.Topics
                .Include(c => c.Chapter)
                .Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation)
                .Include(a => a.ArtifactTypes)
                    .ThenInclude(at => at.Artifacts.Where(ar => ar.Name.ToLower().Contains(artifactName.ToLower())))
                .Where(t => t.ArtifactTypes.Any(at => at.Artifacts.Any(ar => ar.Name.ToLower().Contains(artifactName.ToLower()))));
        }

    }
}
