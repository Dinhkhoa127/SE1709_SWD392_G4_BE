using Azure;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using BiologyRecognition.DTOs.Article;
using BiologyRecognition.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Implement
{
    public class ArticleService : IArticleService
    {
        private readonly ArticleRepository _articleRepository;
        public ArticleService()
        {
            _articleRepository = new ArticleRepository();
        }
        public Task<int> UpdateWithArtifactsAsync(UpdateArticleDTO dto, List<Artifact> newArtifacts)
        {
            return _articleRepository.UpdateWithArtifactsAsync(dto, newArtifacts);
        }


        public async Task<PagedResult<Article>> GetAllAsync(int page, int pageSize)
        {
           var query = _articleRepository.GetAllAsync();
            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Article>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<PagedResult<Article>> GetArticlesByArtifactIdAsync(int artifactId, int page, int pageSize)
        {
            var query = _articleRepository.GetArticlesByArtifactIdAsync(artifactId);

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Article>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public Task<Article> GetByIdAsync(int id)
        {
            return _articleRepository.GetByIdAsync(id);
        }

        public Task<int> UpdateAsync(Article article)
        {
            return _articleRepository.UpdateAsync(article);
        }
        public Task<int> CreateWithArtifactsAsync(Article article, List<Artifact> artifacts)
        {
            return _articleRepository.CreateWithArtifactsAsync(article, artifacts);
        }

        public async Task<PagedResult<Article>> GetListArticleByArtifactNameAsync(string name, int page, int pageSize)
        {
            var query = _articleRepository.GetListArticleByArtifactNameAsync(name);
            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Article>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<List<Article>> GetArticlesByArtifactIdAsync(int artifactId)
        {
            return await _articleRepository.GetArticlesByArtifactIdAsync(artifactId).ToListAsync();
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return await _articleRepository.GetAllAsync().ToListAsync();
        }

        public async Task<List<Article>> GetListArticleByArtifactNameAsync(string name)
        {
            return await _articleRepository.GetListArticleByArtifactNameAsync(name).ToListAsync();
        }
    }
}
