using BiologyRecognition.Domain.DBContext;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Article;
using BloodDonation.Repositories.NhanNB.Basic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Infrastructure
{
    public class ArticleRepository : GenericRepository<Article>
    {
        public ArticleRepository() { }
        public ArticleRepository(SE1709_SWD392_G4_BiologyRecognitionSystemContext context) => _context = context;


        public async Task<List<Article>> GetArticlesByArtifactIdAsync(int artifactId)
        {
            return await _context.Articles.Include(aa => aa.Artifacts).Include(aa => aa.ModifiedByNavigation).Include(aa => aa.CreatedByNavigation)
.Where(a => a.Artifacts.Any(ar => ar.ArtifactId == artifactId)).ToListAsync();
        }
       
        public async Task<int> UpdateAsync(Article article)
        {
            article.Artifacts = null;
            article.CreatedByNavigation = null;
            article.ModifiedByNavigation = null;
            var existing = await _context.Chapters.FindAsync(article.ArticleId);
            if (existing == null) return 0;

            _context.Articles.Update(article);

            return await _context.SaveChangesAsync();
        }
        public async Task<List<Article>> GetAllAsync()
        {
            return await _context.Articles.Include(aa => aa.Artifacts).Include(aa => aa.ModifiedByNavigation).Include(aa => aa.CreatedByNavigation)
               .ToListAsync();
        }

        public async Task<int> CreateWithArtifactsAsync(Article article, List<Artifact> artifacts)
        {
            foreach (var artifact in artifacts)
            {
                _context.Attach(artifact); 
                article.Artifacts.Add(artifact);
            }

            _context.Articles.Add(article);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateWithArtifactsAsync(UpdateArticleDTO dto, List<Artifact> newArtifacts)
        {
            var existingArticle = await _context.Articles
                .AsTracking()
                .Include(a => a.Artifacts)
                .FirstOrDefaultAsync(a => a.ArticleId == dto.ArticleId);

            if (existingArticle == null)
                return 0;

            // Cập nhật thông tin cơ bản
            existingArticle.Title = dto.Title;
            existingArticle.Content = dto.Content;
            existingArticle.ModifiedBy = dto.ModifiedBy;
            existingArticle.ModifiedDate = DateTime.Now;

            // Xử lý thay đổi Artifact
            var currentIds = existingArticle.Artifacts.Select(a => a.ArtifactId).ToList();
            var newIds = newArtifacts.Select(a => a.ArtifactId).ToList();

            // Load danh sách Artifact hiện có
            await _context.Entry(existingArticle)
                .Collection(e => e.Artifacts)
                .LoadAsync();
            existingArticle.Artifacts.Clear();

     

            // Thêm lại các artifact mới từ ID
            foreach (var artifactId in newArtifacts.Select(a => a.ArtifactId).Distinct())
            {
                var trackedArtifact = await _context.Artifacts.FindAsync(artifactId);
                if (trackedArtifact != null)
                {
                    existingArticle.Artifacts.Add(trackedArtifact);
                }
            }

            return await _context.SaveChangesAsync();
        }


        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles.Include(aa => aa.Artifacts).Include(aa => aa.ModifiedByNavigation).Include(aa => aa.CreatedByNavigation)
               .FirstOrDefaultAsync(h => h.ArticleId == id);
        }

        public async Task<List<Article>> GetListArticleByArtifactNameAsync(string name)
        {
            return await _context.Articles.Include(aa => aa.Artifacts)
                .Where(a => a.Artifacts.Any(ar => ar.Name.Contains(name))).ToListAsync();
        }
    }
}
