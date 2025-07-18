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
            return _repository.GetAllAsync().ToListAsync();
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
            return _repository.GetListChaptersByContainNameAsync(name).ToListAsync();
        }

        public Task<int> UpdateAsync(Chapter chapter)
        {
            return _repository.UpdateAsync(chapter);
        }
        public Task<List<Chapter>> GetListChaptersBySubjectIdAsync(int id)
        {
            return _repository.GetListChaptersBySubjectIdAsync(id).ToListAsync();
        }


        public async Task<PagedResult<Chapter>> GetListChaptersByContainNameAsync(string name, int page, int pageSize)
        {
            var query = _repository.GetListChaptersByContainNameAsync(name);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Chapter>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Chapter>> GetListChaptersBySubjectIdAsync(int id, int page, int pageSize)
        {
            var query = _repository.GetListChaptersBySubjectIdAsync(id);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Chapter>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Chapter>> GetAllAsync(int page, int pageSize)
        {
            var query = _repository.GetAllAsync();
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Chapter>
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
