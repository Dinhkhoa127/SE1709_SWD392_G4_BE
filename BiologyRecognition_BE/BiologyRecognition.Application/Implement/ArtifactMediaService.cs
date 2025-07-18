using Azure;
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
    public class ArtifactMediaService : IArtifactMediaService
    {
        private readonly ArtifactMediaRepository _repository;
        public ArtifactMediaService() => _repository ??= new ArtifactMediaRepository();
        public Task<int> CreateAsync(ArtifactMedia artifactMedia)
        {
            return _repository.CreateAsync(artifactMedia);
        }

        public Task<bool> DeleteAsync(ArtifactMedia artifactMedia)
        {
            return _repository.RemoveAsync(artifactMedia);
        }

        public async Task<PagedResult<ArtifactMedia>> GetAllAsync(int page, int pageSize)
        {
            var query = _repository.GetAllAsync();
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ArtifactMedia>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<List<ArtifactMedia>> GetAllAsync()
        {
            return await _repository.GetAllAsync().ToListAsync();
        }

        public Task<ArtifactMedia> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<PagedResult<ArtifactMedia>> GetListArtifactMediaByArtifactIdAsync(int id, int page, int pageSize)
        {
            var query = _repository.GetListArtifactMediaByArtifactIdAsync(id);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ArtifactMedia>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactIdAsync(int id)
        {
            return await _repository.GetListArtifactMediaByArtifactIdAsync(id).ToListAsync();
        }

        public async Task<PagedResult<ArtifactMedia>> GetListArtifactMediaByArtifactNameAsync(string name, int page, int pageSize)
        {
            var query = _repository.GetListArtifactMediaByArtifactNameAsync(name);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ArtifactMedia>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<List<ArtifactMedia>> GetListArtifactMediaByArtifactNameAsync(string name)
        {
           return await _repository.GetListArtifactMediaByArtifactNameAsync(name).ToListAsync();
        }

        public async Task<PagedResult<ArtifactMedia>> GetListArtifactMediaByTypeAsync(string type, int page, int pageSize)
        {
            var query = _repository.GetListArtifactMediaByTypeAsync(type);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ArtifactMedia>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<List<ArtifactMedia>> GetListArtifactMediaByTypeAsync(string type)
        {
          return await _repository.GetListArtifactMediaByTypeAsync(type).ToListAsync();
        }

        public Task<int> UpdateAsync(ArtifactMedia artifactMedia)
        {
            return _repository.UpdateAsync(artifactMedia);
        }
    }
}
