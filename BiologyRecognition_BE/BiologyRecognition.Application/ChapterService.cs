using BiologyRecognition.Domain.Entities;
using BiologyRecognition.Infrastructure;
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
        public Task<int> CreateAsync(Chapter subject)
        {
            return _repository.CreateAsync(subject);
        }

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

        public Task<int> UpdateAsync(Chapter subject)
        {
            return _repository.UpdateAsync(subject);
        }
    }
}
