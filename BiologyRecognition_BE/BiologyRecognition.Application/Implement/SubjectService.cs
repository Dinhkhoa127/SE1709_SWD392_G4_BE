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
    public class SubjectService : ISubjectService
    {
        private readonly SubjectRepository _repository;
        public SubjectService() => _repository ??= new SubjectRepository();

        public Task<Subject> GetSubjectByNameAsync(string name)
        {
            return _repository.GetSubjectByNameAsync(name);
        }

        public Task<List<Subject>> GetAllAsync()
        {
            return _repository.GetAllAsync().ToListAsync();
        }

        public Task<Subject> GetSubjectByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<int> CreateAsync(Subject subject)
        {
            return _repository.CreateAsync(subject);
        }

        public Task<int> UpdateAsync(Subject subject)
        {
            return _repository.UpdateAsync(subject);
        }

        public Task<List<Subject>> GetListSubjectByContainNameAsync(string name)
        {
            return _repository.GetListSubjectByContainNameAsync(name).ToListAsync();
        }
        public Task<bool> DeleteAsync(Subject subject)
        {
            return _repository.RemoveAsync(subject);
        }

        public async Task<PagedResult<Subject>> GetAllAsync(int page, int pageSize)
        {
            var query = _repository.GetAllAsync();
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Subject>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Subject>> GetListSubjectByContainNameAsync(string name, int page, int pageSize)
        {
            var query = _repository.GetListSubjectByContainNameAsync(name);
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Subject>
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
