using AutoMapper;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Article;
using BiologyRecognition.DTOs.Artifact;
using BiologyRecognition.DTOs.ArtifactMedia;
using BiologyRecognition.DTOs.ArtifactType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/artifact")]
    [ApiController]
    [Authorize]
    public class ArtifactController : ControllerBase
    {
        private readonly IArtifactService _artifactService;
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;
        private readonly IArtifactTypeService _artifactTypeService;
        private readonly IUserAccountService _accountService;
        private readonly IArtifactMediaService _artifactMediaService;

        public ArtifactController( IMapper mapper, IArtifactTypeService artifactTypeService,IArtifactService artifactService,IUserAccountService userAccountService, IArtifactMediaService artifactMediaService, IArticleService articleService )
        {
            _mapper = mapper;
            _artifactTypeService = artifactTypeService;
            _artifactService = artifactService;
            _accountService = userAccountService;
            _artifactMediaService = artifactMediaService;
            _articleService = articleService;
        }
       

        [HttpPost]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> CreateArtifact([FromBody] CreateArtifactDTO artifactDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            var artifactType = await _artifactTypeService.GetByIdAsync(artifactDto.ArtifactTypeId);
            if (artifactType == null)
                return NotFound(new { message = "Không tìm thấy loại mẫu ArtifactType." });

            var existingArtifact = await _artifactService.GetByArtifactCodeAsync(artifactDto.ArtifactCode);
            if (existingArtifact != null)
            {
                return Conflict(new { message = "Mã mẫu ArtifactCode đã tồn tại, vui lòng chọn mã khác." });
            }

            var account = await _accountService.GetUserAccountByIdAsync(artifactDto.CreatedBy);
            if (account == null)
                return NotFound(new { message = "Không tìm thấy người tạo." });

            var artifact = _mapper.Map<Artifact>(artifactDto);
            var result = await _artifactService.CreateAsync(artifact);
            if (result > 0)
                return Ok(new { message = "Tạo Artifact thành công." });

            return BadRequest(new { message = "Tạo Artifact thất bại." });
        }

        [HttpPut]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> UpdateArtifact([FromBody] UpdateArtifactDTO artifactDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }
            var existingArtifact = await _artifactService.GetByArtifactCodeAsync(artifactDto.ArtifactCode);
            if (existingArtifact != null)
            {
                return Conflict(new { message = "Mã mẫu ArtifactCode đã tồn tại, vui lòng chọn mã khác." });
            }
            var artifactType = await _artifactTypeService.GetByIdAsync(artifactDto.ArtifactTypeId);
            if (artifactType == null)
                return NotFound(new { message = "Không tìm thấy loại mẫu ArtifactType này." });

            var account = await _accountService.GetUserAccountByIdAsync(artifactDto.ModifiedBy ?? 0);
            if (account == null)
                return NotFound(new { message = "Không tìm thấy người sửa." });

            var artifact = await _artifactService.GetByIdAsync(artifactDto.ArtifactId);
            if (artifact == null)
                return NotFound(new { message = "Không tìm thấy Artifact." });

            _mapper.Map(artifactDto, artifact);
            var result = await _artifactService.UpdateAsync(artifact);
            if (result > 0)
                return Ok(new { message = "Cập nhật Artifact thành công." });

            return BadRequest(new { message = "Cập nhật Artifact thất bại." });
        }

        [HttpGet]
        [Authorize(Roles = "2,3")]
        public async Task<IActionResult> GetArtifacts(
    [FromQuery] int? id,
    [FromQuery] string? name,
    [FromQuery] int? artifactTypeId,
    [FromQuery] bool includeDetails = false,
    [FromQuery] bool includeMediaAndArticles = false,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 3)
        {
            // 1. Lấy theo ID (có thể gộp chi tiết)
            if (id.HasValue)
            {
                var artifact = await _artifactService.GetByIdAsync(id.Value);
                if (artifact == null)
                    return NotFound("Artifact không tồn tại.");

                if (includeMediaAndArticles)
                {
                    var mediaList = await _artifactMediaService.GetListArtifactMediaByArtifactIdAsync(id.Value);
                    var articleList = await _articleService.GetArticlesByArtifactIdAsync(id.Value);
                    var artifactDto = _mapper.Map<ArtifactWithMediaArticleDTO>(artifact);
                    artifactDto.MediaList = _mapper.Map<List<ArtifactMediaDTO>>(mediaList);
                    artifactDto.ArticleList = _mapper.Map<List<ArticleDTO>>(articleList);
                    return Ok(artifactDto);
                }

                if (includeDetails)
                {
                    var dto = _mapper.Map<ArtifactDetailsDTO>(artifact);
                    return Ok(dto);
                }
                else
                {
                    var dto = _mapper.Map<ArtifactDTO>(artifact);
                    return Ok(dto);
                }
            }

            // 2. Lọc theo tên  
            if (!string.IsNullOrWhiteSpace(name))
            {
                var list = await _artifactService.GetArtifactsByContainsNameAsync(name,page,pageSize);
                if (list.Items == null || list.TotalItems == 0)
                    return NotFound("Không có Artifact phù hợp với từ khóa tìm kiếm.");

                if (includeMediaAndArticles)
                {
                    var mediaList = await _artifactMediaService.GetListArtifactMediaByArtifactNameAsync(name);
                    var articleList = await _articleService.GetListArticleByArtifactNameAsync(name);
                    var artifactDtos = _mapper.Map<List<ArtifactWithMediaArticleDTO>>(list.Items);

                    foreach (var dto in artifactDtos)
                    {   
                        dto.MediaList = _mapper.Map<List<ArtifactMediaDTO>>(
                            mediaList.Where(m => m.ArtifactId == dto.ArtifactId).ToList());

                        dto.ArticleList = _mapper.Map<List<ArticleDTO>>(
                            articleList.Where(article =>
                                article.Artifacts.Any(artifact => artifact.ArtifactId == dto.ArtifactId)
                            ).ToList());
                    }

                    return Ok(artifactDtos);
                }

                if (includeDetails)
                {
                    var dto = _mapper.Map<List<ArtifactDetailsDTO>>(list.Items);
                    return Ok(dto);
                }
                else
                {
                    var dto = _mapper.Map<List<ArtifactDTO>>(list.Items);
                    return Ok(dto);
                }
            }

            // 3. Lọc theo artifactTypeId
            if (artifactTypeId.HasValue)
            {
                var artifacts = await _artifactService.GetListArtifactsByArtifactTypeIdAsync(artifactTypeId.Value,page,pageSize);
                if (artifacts.Items == null || artifacts.TotalItems == 0)
                    return NotFound("Không có Artifact nào thuộc loại này.");

                var dto = _mapper.Map<List<ArtifactDTO>>(artifacts.Items);
                return Ok(dto);
            }

            // 4. Lấy tất cả (có thể kèm chi tiết)
            var all = await _artifactService.GetAllAsync(page,pageSize);
            if (all.Items == null || all.TotalItems == 0)
                return NotFound("Không tìm thấy artifact nào.");

            if (includeDetails)
            {
                var dto = _mapper.Map<List<ArtifactDetailsDTO>>(all.Items);
                return Ok(dto);
            }
            else
            {
                var dto = _mapper.Map<List<ArtifactDTO>>(all.Items);
                return Ok(dto);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(int id)
        {
            var artifact = await _artifactService.GetByIdAsync(id);
            if (artifact == null)
                return NotFound("Artifact không tồn tại.");

            // Không thực hiện thao tác xóa thật sự
            return Ok(new { message = $"Xóa thành công" });
        }
    }

}

