using BiologyRecognition.Domain.Entities;
using BiologyRecognition.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public class ChapterService : IChapterService
    {
        private readonly ChapterRepository _repository;
        public ChapterService() => _repository ??= new ChapterRepository();
        public async Task<int> CreateAsync(Chapter chapter)
        {
            return await _repository.CreateAsync(chapter);
        }

        // Không cần async/await nếu chỉ return thẳng
        public Task<List<Chapter>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Chapter> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<Chapter> GetChapterByNameAsync(string name)
        {
            return _repository.GetChapterByNameAsync(name);
        }

        public Task<List<Chapter>> GetListChaptersByContainNameAsync(string name)
        {
            return _repository.GetListChaptersByContainNameAsync(name);
        }

        public Task<int> UpdateAsync(Chapter chapter)
        {
            return _repository.UpdateAsync(chapter);
        }
        public  Task<List<Chapter>> GetListChaptersBySubjectIdAsync(int id)
        {
            return _repository.GetListChaptersBySubjectIdAsync(id);
        }
    }
}
