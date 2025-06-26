using BiologyRecognition.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public interface IChapterService
    {
  

        Task<int> CreateAsync(Chapter subject);
        Task<int> UpdateAsync(Chapter subject);
        Task<List<Chapter>> GetAllAsync();
        Task<Chapter> GetChapterByNameAsync(string name);
        Task<List<Chapter>> GetListChaptersByContainNameAsync(string name);
        Task<Chapter> GetByIdAsync(int id);
    }
}
