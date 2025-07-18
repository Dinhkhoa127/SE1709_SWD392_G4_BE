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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BiologyRecognition.Application.Implement
{
    public class ArtifactService : IArtifactService
    {

        private readonly ArtifactRepository _artifactRepository;
        public ArtifactService() => _artifactRepository ??= new ArtifactRepository();

        public Task<int> CreateAsync(Artifact artifact)
        {
            return _artifactRepository.CreateAsync(artifact);
        }

        public async Task<List<Artifact>> GetAllAsync()
        {
            return await _artifactRepository.GetAllAsync().ToListAsync();
        }

        public async Task<List<Artifact>> GetArtifactsByContainsNameAsync(string name)
        {
            return await _artifactRepository.GetArtifactsByContainsNameAsync(name).ToListAsync();
        }

        public Task<Artifact> GetByIdAsync(int id)
        {
            return _artifactRepository.GetByIdAsync(id);
        }

        public async Task<List<Artifact>> GetListArtifactsByArtifactTypeIdAsync(int id)
        {
            return  await _artifactRepository.GetListArtifactsByArtifactTypeIdAsync(id).ToListAsync();
        }

        public Task<int> UpdateAsync(Artifact artifact)
        {
            return _artifactRepository.UpdateAsync(artifact);
        }
        public async Task<List<Artifact>> GetListArtifactsByListIdsAsync(List<int> artifactIds)
        {
            return await _artifactRepository.GetListArtifactsByListIdsAsync(artifactIds).ToListAsync();
        }

        public async Task<PagedResult<Artifact>> GetAllAsync(int page, int pageSize)
        {
            var query = _artifactRepository.GetAllAsync();
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Artifact>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Artifact>> GetArtifactsByContainsNameAsync(string name, int page, int pageSize)
        {
            var query = _artifactRepository.GetArtifactsByContainsNameAsync(name);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Artifact>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Artifact>> GetListArtifactsByArtifactTypeIdAsync(int id, int page, int pageSize)
        {
            var query = _artifactRepository.GetListArtifactsByArtifactTypeIdAsync(id);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Artifact>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Artifact>> GetListArtifactsByListIdsAsync(List<int> artifactIds, int page, int pageSize)
        {
            var query = _artifactRepository.GetListArtifactsByListIdsAsync(artifactIds);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Artifact>
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
