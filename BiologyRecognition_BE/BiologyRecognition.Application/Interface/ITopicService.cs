using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface ITopicService
    {
        Task<int> CreateAsync(Topic topic);
        Task<int> UpdateAsync(Topic topic);
        Task<List<Topic>> GetAllAsync();
        Task<PagedResult<Topic>> GetAllAsync(int page, int pageSize);
        Task<List<Topic>> GetListTopicsByContainNameAsync(string name);
        Task<PagedResult<Topic>> GetListTopicsByContainNameAsync(string name, int page, int pageSize);
        Task<Topic> GetByIdAsync(int id);
        Task<List<Topic>> GetListTopicsByChapterIdAsync(int id);
        Task<PagedResult<Topic>> GetListTopicsByChapterIdAsync(int id, int page, int pageSize);
        Task<List<Topic>> GetListTopicsByArtifactNameAsync(string artifactName);
        Task<PagedResult<Topic>> GetListTopicsByArtifactNameAsync(string artifactName, int page, int pageSize);
    }
}

