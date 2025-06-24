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
            return await _context.Subjects.FirstOrDefaultAsync(u => u.Name == name);
        }
        public async Task<List<Subject>> GetListSubjectByContainNameAsync(string name)
        {
            return await _context.Subjects
                .Where(u => u.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

      
    }
}
