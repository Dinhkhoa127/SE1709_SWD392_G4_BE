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

        public async Task<List<Topic>> GetTopicByContainsNameAsync(string name)
        {
            return await _context.Topics.Where(u => u.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
       


    }
}
