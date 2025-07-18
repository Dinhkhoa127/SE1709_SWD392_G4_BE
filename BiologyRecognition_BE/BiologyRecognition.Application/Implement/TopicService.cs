using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using BiologyRecognition.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Implement
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
            return _repository.GetAllAsync().ToListAsync();
        }

        public Task<Topic> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<Topic>> GetListTopicsByContainNameAsync(string name)
        {
            return _repository.GetTopicsByContainsNameAsync(name).ToListAsync();
        }

        public Task<int> UpdateAsync(Topic topic)
        {
            return _repository.UpdateAsync(topic);
        }
        public Task<List<Topic>> GetListTopicsByChapterIdAsync(int id)
        {
            return _repository.GetListTopicsByChapterIdAsync(id).ToListAsync();
        }

        public Task<List<Topic>> GetListTopicsByArtifactNameAsync(string artifactName)
        {
            return _repository.GetListTopicsByArtifactNameAsync(artifactName).ToListAsync();
        }

        public async Task<PagedResult<Topic>> GetAllAsync(int page, int pageSize)
        {
            var query = _repository.GetAllAsync();
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Topic>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Topic>> GetListTopicsByContainNameAsync(string name, int page, int pageSize)
        {
            var query = _repository.GetTopicsByContainsNameAsync(name);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Topic>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Topic>> GetListTopicsByChapterIdAsync(int id, int page, int pageSize)
        {
            var query = _repository.GetListTopicsByChapterIdAsync(id);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Topic>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Topic>> GetListTopicsByArtifactNameAsync(string artifactName, int page, int pageSize)
        {
            var query = _repository.GetListTopicsByArtifactNameAsync(artifactName);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Topic>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }
    }
}
