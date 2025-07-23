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
    public class ChapterRepository : GenericRepository<Chapter>
    {
        public ChapterRepository() { }
        public ChapterRepository(SE1709_SWD392_G4_BiologyRecognitionSystemContext context) => _context = context;

        public async Task<Chapter> GetChapterByNameAsync(string name)
        {
            return await _context.Chapters.Include(c => c.Subject).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).FirstOrDefaultAsync(u => u.Name == name);
        }

        public IQueryable<Chapter> GetListChaptersByContainNameAsync(string name)
        {
            return  _context.Chapters.Include(c => c.Subject).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation)
                .Where(u => u.Name.ToLower().Contains(name.ToLower()));
        }
        public async Task<int> UpdateAsync(Chapter chapter)
        {
            chapter.ModifiedByNavigation = null;
            chapter.Subject = null;
            var existing = await _context.Chapters.FindAsync(chapter.ChapterId);
            if (existing == null) return 0;

            _context.Chapters.Update(chapter);

            return await _context.SaveChangesAsync();
        }
        public IQueryable<Chapter> GetAllAsync()
        {
            return _context.Chapters.Include(c => c.Subject).Include(c => c.CreatedByNavigation).Include(c => c.ModifiedByNavigation);
        }
        public IQueryable<Chapter> GetListChaptersBySubjectIdAsync(int id)
        {
            return _context.Chapters.Include(c => c.Subject).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation)
                .Where(u => u.SubjectId == id);
        }
        public async Task<Chapter> GetByIdAsync(int id)
        {
            return await _context.Chapters.Include(c => c.Subject).Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).FirstOrDefaultAsync(h => h.ChapterId == id);
        }


    }
}
