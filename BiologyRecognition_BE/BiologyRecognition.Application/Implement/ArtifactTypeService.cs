using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.DBContext;
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
    public class ArtifactTypeService : IArtifactTypeService
    {
        private readonly ArtifactTypeRepository _repository;
        public ArtifactTypeService() => _repository ??= new ArtifactTypeRepository();
        public Task<int> CreateAsync(ArtifactType artifactType)
        {
            return _repository.CreateAsync(artifactType);
        }

        public Task<List<ArtifactType>> GetAllAsync()
        {
            return _repository.GetAllAsync().ToListAsync();
        }

        public async Task<PagedResult<ArtifactType>> GetAllAsync(int page, int pageSize)
        {
            var query = _repository.GetAllAsync();
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ArtifactType>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public Task<List<ArtifactType>> GetArtifactTypesByContainsNameAsync(string name)
        {
            return _repository.GetArtifactTypesByContainsNameAsync(name).ToListAsync();
        }

        public async Task<PagedResult<ArtifactType>> GetArtifactTypesByContainsNameAsync(string name, int page, int pageSize)
        {
            var query = _repository.GetArtifactTypesByContainsNameAsync(name);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ArtifactType>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public Task<ArtifactType> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<ArtifactType>> GetListArtifactTypesByTopicIdAsync(int id)
        {
            return _repository.GetListArtifactTypesByTopicIdAsync(id).ToListAsync();
        }

        public async Task<PagedResult<ArtifactType>> GetListArtifactTypesByTopicIdAsync(int id, int page, int pageSize)
        {
            var query = _repository.GetListArtifactTypesByTopicIdAsync(id);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ArtifactType>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public Task<int> UpdateAsync(ArtifactType artifactType)
        {
            return _repository.UpdateAsync(artifactType);
        }
    }
}
