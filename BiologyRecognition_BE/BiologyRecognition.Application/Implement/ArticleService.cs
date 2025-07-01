using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
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


        public Task<List<Article>> GetAllAsync()
        {
            return _articleRepository.GetAllAsync();
        }

        public Task<List<Article>> GetArticlesByArtifactIdAsync(int artifactId)
        {
            return _articleRepository.GetArticlesByArtifactIdAsync(artifactId);
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

        public Task<List<Article>> GetListArticleByArtifactNameAsync(string name)
        {
            return _articleRepository.GetListArticleByArtifactNameAsync(name);
        }
    }
}
