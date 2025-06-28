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
    public class TopicService : ITopicService
    {
        private readonly TopicRepository _repository;
        public TopicService() => _repository ??= new TopicRepository();
        public Task<int> CreateAsync(Topic topic)
        {
            return _repository.CreateAsync(topic);
        }

        public Task<List<Topic>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Topic> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
         }

        public Task<List<Topic>> GetListTopicsByContainNameAsync(string name)
        {
            return _repository.GetTopicsByContainsNameAsync(name);
        }

        public Task<int> UpdateAsync(Topic topic)
        {
            return _repository.UpdateAsync(topic);
        }
        public Task<List<Topic>> GetListTopicsByChapterIdAsync(int id)
        {
            return _repository.GetListTopicsByChapterIdAsync(id);
        }

        public Task<List<Topic>> GetListTopicsByArtifactNameAsync(string artifactName)
        {
            return _repository.GetListTopicsByArtifactNameAsync(artifactName);
        }
    }
}
