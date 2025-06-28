using AutoMapper;
using BiologyRecognition.Application;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArtifactService _artifactService;
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;
        private readonly IArtifactTypeService _artifactTypeService;
        private readonly IUserAccountService _accountService;
        private readonly IArtifactMediaService _artifactMediaService;

        public ArticleController(IMapper mapper, IArtifactTypeService artifactTypeService, IArtifactService artifactService, IUserAccountService userAccountService, IArtifactMediaService artifactMediaService, IArticleService articleService)
        {
            _mapper = mapper;
            _artifactTypeService = artifactTypeService;
            _artifactService = artifactService;
            _accountService = userAccountService;
            _artifactMediaService = artifactMediaService;
            _articleService = articleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var list = await _articleService.GetAllAsync();
            if (list == null || list.Count == 0)
                return NotFound("Không có bài viết nào.");

            var dto = _mapper.Map<List<ArticleDTO>>(list);
            return Ok(dto);
        }
        [HttpGet("details")]
        public async Task<IActionResult> GetAllArticlesDetails()
        {
            var list = await _articleService.GetAllAsync();
            if (list == null || list.Count == 0)
                return NotFound("Không có bài viết nào.");

            var dto = _mapper.Map<List<ArticleDetailsDTO>>(list);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var entity = await _articleService.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Bài viết không tồn tại.");

            var dto = _mapper.Map<ArticleDTO>(entity);
            return Ok(dto);
        }
        [HttpGet("{id}-details")]
        public async Task<IActionResult> GetArticleDetailsById(int id)
        {
            var entity = await _articleService.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Bài viết không tồn tại.");

            var dto = _mapper.Map<ArticleDetailsDTO>(entity);
            return Ok(dto);
        }

        [HttpGet("by-artifact/{artifactId}")]
        public async Task<IActionResult> GetArticlesByArtifactId(int artifactId)
        {
            var artifact = await _artifactService.GetByIdAsync(artifactId);
            if (artifact == null)
                return NotFound("Không tìm thấy mẫu tương ứng.");

            var list = await _articleService.GetArticlesByArtifactIdAsync(artifactId);
            if (list == null || list.Count == 0)
                return NotFound("Không có bài viết nào liên kết với mẫu này.");

            var dto = _mapper.Map<List<ArticleDTO>>(list);
            return Ok(dto);
        }
        [HttpGet("by-artifactName/{artifactName}")]
        public async Task<IActionResult> GetArticlesByArtifactName(string? artifactName)
        {
            if (string.IsNullOrWhiteSpace(artifactName))
                return BadRequest("Tên mẫu không được để trống.");

            var list = await _articleService.GetListArticleByArtifactNameAsync(artifactName);
            if (list == null || list.Count == 0)
                return NotFound("Không có bài viết nào liên kết với mẫu này.");

            var dto = _mapper.Map<List<ArticleDTO>>(list);
            return Ok(dto);
        }
        [HttpGet("by-artifact-details/{artifactId}")]
        public async Task<IActionResult> GetArticleDetailsByArtifactId(int artifactId)
        {
            var artifact = await _artifactService.GetByIdAsync(artifactId);
            if (artifact == null)
                return NotFound("Không tìm thấy mẫu tương ứng.");

            var list = await _articleService.GetArticlesByArtifactIdAsync(artifactId);
            if (list == null || list.Count == 0)
                return NotFound("Không có bài viết nào liên kết với mẫu này.");

            var dto = _mapper.Map<List<ArticleDetailsDTO>>(list);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

             var user = await _accountService.GetUserAccountByIdAsync(dto.CreatedBy);
            if (user == null) return NotFound("Người tạo không tồn tại.");

            var entity = _mapper.Map<Article>(dto);
            var artifacts = await _artifactService.GetListArtifactsByListIdsAsync(dto.ArtifactIds);
            var result = await _articleService.CreateWithArtifactsAsync(entity, artifacts); 
        
            if (result > 0)
                return Ok(new { message = "Tạo bài viết thành công." });

            return BadRequest(new { message = "Tạo bài viết thất bại." });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArticle([FromBody] UpdateArticleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { message = errors });
            }

            // Kiểm tra bài viết có tồn tại không
            var existingArticle = await _articleService.GetByIdAsync(dto.ArticleId);
            if (existingArticle == null)
                return NotFound("Không tìm thấy bài viết.");

            // Kiểm tra người sửa có tồn tại không
            var user = await _accountService.GetUserAccountByIdAsync(dto.ModifiedBy);
            if (user == null) return NotFound("Người sửa không tồn tại.");

            // Lấy danh sách Artifact từ ID
            var artifactEntities = await _artifactService.GetListArtifactsByListIdsAsync(dto.ArtifactIds);

            // Gọi service cập nhật
            var result = await _articleService.UpdateWithArtifactsAsync(dto, artifactEntities);

            if (result > 0)
                return Ok(new { message = "Cập nhật bài viết thành công." });

            return BadRequest(new { message = "Cập nhật bài viết thất bại." });
        }

    }
}
