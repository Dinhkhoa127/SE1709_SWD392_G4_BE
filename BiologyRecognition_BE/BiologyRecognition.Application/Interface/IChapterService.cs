using BiologyRecognition.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface IChapterService
    {


        Task<int> CreateAsync(Chapter chapter);
        Task<int> UpdateAsync(Chapter chapter);
        Task<List<Chapter>> GetAllAsync();
        Task<Chapter> GetChapterByNameAsync(string name);
        Task<List<Chapter>> GetListChaptersByContainNameAsync(string name);
        Task<Chapter> GetByIdAsync(int id);
        Task<List<Chapter>> GetListChaptersBySubjectIdAsync(int id);
    }
}
