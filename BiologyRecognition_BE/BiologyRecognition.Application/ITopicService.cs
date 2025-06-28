using BiologyRecognition.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public interface ITopicService
    {
        Task<int> CreateAsync(Topic topic);
        Task<int> UpdateAsync(Topic topic);
        Task<List<Topic>> GetAllAsync();
        Task<List<Topic>> GetListTopicsByContainNameAsync(string name);
        Task<Topic> GetByIdAsync(int id);
        Task<List<Topic>> GetListTopicsByChapterIdAsync(int id);
        Task<List<Topic>> GetListTopicsByArtifactNameAsync(string artifactName);
    }
}

