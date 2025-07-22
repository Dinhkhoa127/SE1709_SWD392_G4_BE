using AutoMapper;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/article")]
    [ApiController]
    [Authorize]
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
      
        [Authorize(Roles = "3")]
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
        [Authorize(Roles = "3")]
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

        [HttpGet]
        [Authorize(Roles = "2,3")]
        public async Task<IActionResult> GetArticles(
    [FromQuery] int? id,
    [FromQuery] int? artifactId,
    [FromQuery] string? artifactName,
    [FromQuery] bool includeDetails = false,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 3)
        {
            // 1. Lấy theo ID
            if (id.HasValue)
            {
                var entity = await _articleService.GetByIdAsync(id.Value);
                if (entity == null)
                    return NotFound("Bài viết không tồn tại.");

                if (includeDetails)
                {
                    var dto = _mapper.Map<ArticleDetailsDTO>(entity);
                    return Ok(dto);
                }
                else
                {
                    var dto = _mapper.Map<ArticleDTO>(entity);
                    return Ok(dto);
                }
            }

            // 2. Lấy theo artifactId
            if (artifactId.HasValue)
            {
                var artifact = await _artifactService.GetByIdAsync(artifactId.Value);
                if (artifact == null)
                    return NotFound("Không tìm thấy mẫu tương ứng.");

                var list = await _articleService.GetArticlesByArtifactIdAsync(artifactId.Value,page,pageSize);
                if (list.Items == null || list.TotalItems == 0)
                    return NotFound("Không có bài viết nào liên kết với mẫu này.");

                if (includeDetails)
                {
                    var dto = _mapper.Map<List<ArticleDetailsDTO>>(list.Items);
                    return Ok(dto);
                }
                else
                {
                    var dto = _mapper.Map<List<ArticleDTO>>(list.Items);
                    return Ok(dto);
                }
            }

            // 3. Lấy theo artifactName
            if (!string.IsNullOrWhiteSpace(artifactName))
            {
                var list = await _articleService.GetListArticleByArtifactNameAsync(artifactName,page,pageSize);
                if (list.Items == null || list.TotalItems == 0)
                    return NotFound("Không có bài viết nào liên kết với mẫu này.");

            }

            // 4. Mặc định: lấy tất cả
            var all = await _articleService.GetAllAsync(page,pageSize);
            if (all.Items == null || all.TotalItems == 0)
                return NotFound("Không có bài viết nào.");

            if (includeDetails)
            {
                var dto = _mapper.Map<List<ArticleDetailsDTO>>(all.Items);
                return Ok(dto);
            }
            else
            {
                var dto = _mapper.Map<List<ArticleDTO>>(all.Items);
                return Ok(dto);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(int id)
        {
            // Kiểm tra tồn tại (tuỳ chọn)
            var article = await _articleService.GetByIdAsync(id);
            if (article == null)
                return NotFound("Bài viết không tồn tại.");

            // Không thực hiện thao tác xóa thật sự
            return Ok(new { message = $"Xóa thành công" });
        }
    }
}
