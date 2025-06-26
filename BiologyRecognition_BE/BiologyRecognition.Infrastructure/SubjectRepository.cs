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
    public class SubjectRepository : GenericRepository<Subject>
    {
        public SubjectRepository() { }
        public SubjectRepository(SE1709_SWD392_G4_BiologyRecognitionSystemContext context) => _context = context;

        public async Task<Subject> GetSubjectByNameAsync(string name)
        {
            return await _context.Subjects
                .Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation)
                .FirstOrDefaultAsync(u => u.Name == name);
        }
        public async Task<List<Subject>> GetListSubjectByContainNameAsync(string name)
        {
            return await _context.Subjects.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation)
                .Where(u => u.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }
        public async Task<List<Subject>> GetAllAsync()
        {
            return await _context.Subjects.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).ToListAsync();
        }
        public async Task<Subject> GetByIdAsync(int id)
        {
            return await _context.Subjects.Include(c => c.CreatedByNavigation)
                .Include(c => c.ModifiedByNavigation).FirstOrDefaultAsync(h => h.SubjectId == id);
        }
        public async Task<int> UpdateAsync(Subject subject)
        {
            subject.ModifiedByNavigation = null;
            var existing = await _context.Chapters.FindAsync(subject.SubjectId);
            if (existing == null) return 0;

            _context.Subjects.Update(subject);

            return await _context.SaveChangesAsync();
        }
    }
}
