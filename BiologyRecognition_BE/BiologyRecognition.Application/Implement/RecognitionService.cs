using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using BiologyRecognition.DTOs.Recognition;
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

        public async Task<int> DeleteExpiredRecognitionsAsync(CancellationToken cancellationToken = default)
        {
            return await _repository.DeleteExpiredRecognitionsAsync(cancellationToken);
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
                .OrderByDescending(c => c.RecognizedAt)
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
            return _repository.GetRecognitionUserByIdAsync(userId).OrderByDescending(c => c.RecognizedAt).ToListAsync();
        }

        public async Task<PagedResult<Recognition>> GetRecognitionUserByIdAsync(int userId, int page, int pageSize)
        {
            var query = _repository.GetRecognitionUserByIdAsync(userId);
            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.RecognizedAt)
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

        public async Task<Recognition> CreateFailedRecognition(ImageDTO imageDTO, double confidence)
        {
            var recognition = new Recognition
            {
                UserId = imageDTO.UserId,
                Artifact = null,
                ArtifactId = null,
                ImageUrl = imageDTO.ImageUrl,
                RecognizedAt = DateTime.Now,
                ConfidenceScore = confidence,
                AiResult = "Không nhận dạng được chính xác",
                Status = "FAILED"
            };
            return await Task.FromResult(recognition);
        }

        public async Task<Recognition> CreateSuccessRecognition(ImageDTO imageDTO, Artifact firstArtifact, double confidence)
        {
            var recognition = new Recognition
            {
                UserId = imageDTO.UserId,
                ArtifactId = firstArtifact.ArtifactId,
                ImageUrl = imageDTO.ImageUrl,
                RecognizedAt = DateTime.Now,
                ConfidenceScore = confidence,
                AiResult = firstArtifact.Name,
                Status = "COMPLETED"
            };
            return await Task.FromResult(recognition);
        }
    }
}
