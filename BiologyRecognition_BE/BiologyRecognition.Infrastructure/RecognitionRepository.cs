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
    public class RecognitionRepository:GenericRepository<Recognition>
    {
        public RecognitionRepository() { }
        public RecognitionRepository(SE1709_SWD392_G4_BiologyRecognitionSystemContext context) => _context = context;


        public IQueryable<Recognition> GetAllAsync()
        {
            return  _context.Recognitions.Include(r => r.Artifact);
        }
        public async Task<Recognition> GetByIdAsync(int id)
        {
            return await _context.Recognitions.Include(r => r.Artifact)
                .FirstOrDefaultAsync(r => r.RecognitionId == id) ;
        }

        public IQueryable<Recognition> GetRecognitionUserByIdAsync(int userId)
        {
            return  _context.Recognitions.Include(r => r.Artifact)
                .Where(r => r.UserId == userId);
        }
        public async Task<int> DeleteExpiredRecognitionsAsync(CancellationToken cancellationToken = default)
        {
            var expiredRecognitions = await _context.Recognitions
                .Where(r => r.RecognizedAt != null && r.RecognizedAt <= DateTime.Now.AddDays(-90))
                .ToListAsync(cancellationToken);

            if (expiredRecognitions.Any())
            {
                _context.Recognitions.RemoveRange(expiredRecognitions);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return expiredRecognitions.Count;
        }
    }
}
