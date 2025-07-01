using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface IArticleService
    {
        Task<List<Article>> GetArticlesByArtifactIdAsync(int artifactId);
        Task<int> UpdateWithArtifactsAsync(UpdateArticleDTO dto, List<Artifact> newArtifacts);
        Task<int> UpdateAsync(Article article);

        Task<List<Article>> GetAllAsync();

        Task<Article> GetByIdAsync(int id);
        Task<int> CreateWithArtifactsAsync(Article article, List<Artifact> artifacts);
        Task<List<Article>> GetListArticleByArtifactNameAsync(string name);
    }
}
