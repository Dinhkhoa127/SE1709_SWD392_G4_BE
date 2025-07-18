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
    public class RecognitionService : IRecognitionService
    {
        private readonly RecognitionRepository _repository;
        public RecognitionService() => _repository ??= new RecognitionRepository();

        public Task<int> CreatAsync(Recognition recognition)
        {
            return _repository.CreateAsync(recognition);
        }

        public Task<List<Recognition>> GetAllAsync()
        {
            return _repository.GetAllAsync().ToListAsync();
        }

        public async Task<PagedResult<Recognition>> GetAllAsync(int page, int pageSize)
        {
            var query = _repository.GetAllAsync();
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Recognition>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public Task<Recognition> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<Recognition>> GetRecognitionUserByIdAsync(int userId)
        {
            return _repository.GetRecognitionUserByIdAsync(userId).ToListAsync();
        }

        public async Task<PagedResult<Recognition>> GetRecognitionUserByIdAsync(int userId, int page, int pageSize)
        {
            var query = _repository.GetRecognitionUserByIdAsync(userId);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Recognition>
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
