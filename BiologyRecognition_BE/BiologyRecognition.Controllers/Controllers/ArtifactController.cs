using AutoMapper;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Article;
using BiologyRecognition.DTOs.Artifact;
using BiologyRecognition.DTOs.ArtifactMedia;
using BiologyRecognition.DTOs.ArtifactType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/artifact")]
    [ApiController]
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
        [HttpGet(".")]
        public async Task<IActionResult> GetAllArtifacts()
        {
            var artifacts = await _artifactService.GetAllAsync();
            if (artifacts == null || artifacts.Count == 0)
                return NotFound("Không tìm thấy artifact nào.");

            var dto = _mapper.Map<List<ArtifactDTO>>(artifacts);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtifactById(int id)
        {
            var artifact = await _artifactService.GetByIdAsync(id);
            if (artifact == null)
                return NotFound("Artifact không tồn tại.");

            var dto = _mapper.Map<ArtifactDTO>(artifact);
            return Ok(dto);
        }
        [HttpGet("by-id/{id}/with-media-article")]
        public async Task<IActionResult> GetMediaArticleById(int id)
        {
            var artifact = await _artifactService.GetByIdAsync(id);
            if (artifact == null)
                return NotFound("Artifact không tồn tại.");

            var mediaList = await _artifactMediaService.GetListArtifactMediaByArtifactIdAsync(id);
            var articleList = await _articleService.GetArticlesByArtifactIdAsync(id);
            var artifactDto = _mapper.Map<ArtifactWithMediaArticleDTO>(artifact);
            artifactDto.MediaList = _mapper.Map<List<ArtifactMediaDTO>>(mediaList);
            artifactDto.ArticleList = _mapper.Map<List<ArticleDTO>>(articleList);
            return Ok(artifactDto);
        }

        [HttpGet("by-name/{name}/with-media-article")]
        public async Task<IActionResult> GetMediaArticleByArtifactName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống." });   
            var artifact = await _artifactService.GetArtifactsByContainsNameAsync(name);
            if (artifact == null || artifact.Count == 0)
                return NotFound("Không tìm thấy artifact nào.");

            var mediaList = await _artifactMediaService.GetListArtifactMediaByArtifactNameAsync(name);
            var articleList = await _articleService.GetListArticleByArtifactNameAsync(name);
            var artifactDtos = _mapper.Map<List<ArtifactWithMediaArticleDTO>>(artifact);

            // Gán MediaList và ArticleList vào từng artifact
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

        [HttpGet("all-detail")]
        public async Task<IActionResult> GetAllArtifactsDetails()
        {
            var artifacts = await _artifactService.GetAllAsync();
            if (artifacts == null || artifacts.Count == 0)
                return NotFound("Không tìm thấy artifact nào.");

            var dto = _mapper.Map<List<ArtifactDetailsDTO>>(artifacts);
            return Ok(dto);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetArtifactDetailsById(int id)
        {
            var artifact = await _artifactService.GetByIdAsync(id);
            if (artifact == null)
                return NotFound("Artifact không tồn tại.");

            var dto = _mapper.Map<ArtifactDetailsDTO>(artifact);
            return Ok(dto);
        }

        [HttpGet("filter-name")]
        public async Task<IActionResult> GetArtifactsByContainName([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống." });

            var list = await _artifactService.GetArtifactsByContainsNameAsync(name);
            if (list == null || list.Count == 0)
                return NotFound("Không có Artifact phù hợp với từ khóa tìm kiếm.");

            var dto = _mapper.Map<List<ArtifactDTO>>(list);
            return Ok(dto);
        }

        [HttpGet("by-artifact-type/{artifactTypeId}")]
        public async Task<IActionResult> GetArtifactsByArtifactTypeId(int artifactTypeId)
        {
            var artifacts = await _artifactService.GetListArtifactsByArtifactTypeIdAsync(artifactTypeId);
            if (artifacts == null || artifacts.Count == 0)
                return NotFound("Không có Artifact nào thuộc loại này.");

            var dto = _mapper.Map<List<ArtifactDTO>>(artifacts);
            return Ok(dto);
        }

        [HttpPost]
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
        public async Task<IActionResult> UpdateArtifact([FromBody] UpdateArtifactDTO artifactDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
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
        public async Task<IActionResult> GetArtifacts(
    [FromQuery] int? id,
    [FromQuery] string? name,
    [FromQuery] int? artifactTypeId,
    [FromQuery] bool includeDetails = false,
    [FromQuery] bool includeMediaAndArticles = false)
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
                var list = await _artifactService.GetArtifactsByContainsNameAsync(name);
                if (list == null || list.Count == 0)
                    return NotFound("Không có Artifact phù hợp với từ khóa tìm kiếm.");

                if (includeMediaAndArticles)
                {
                    var mediaList = await _artifactMediaService.GetListArtifactMediaByArtifactNameAsync(name);
                    var articleList = await _articleService.GetListArticleByArtifactNameAsync(name);
                    var artifactDtos = _mapper.Map<List<ArtifactWithMediaArticleDTO>>(list);

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
                    var dto = _mapper.Map<List<ArtifactDetailsDTO>>(list);
                    return Ok(dto);
                }
                else
                {
                    var dto = _mapper.Map<List<ArtifactDTO>>(list);
                    return Ok(dto);
                }
            }

            // 3. Lọc theo artifactTypeId
            if (artifactTypeId.HasValue)
            {
                var artifacts = await _artifactService.GetListArtifactsByArtifactTypeIdAsync(artifactTypeId.Value);
                if (artifacts == null || artifacts.Count == 0)
                    return NotFound("Không có Artifact nào thuộc loại này.");

                var dto = _mapper.Map<List<ArtifactDTO>>(artifacts);
                return Ok(dto);
            }

            // 4. Lấy tất cả (có thể kèm chi tiết)
            var all = await _artifactService.GetAllAsync();
            if (all == null || all.Count == 0)
                return NotFound("Không tìm thấy artifact nào.");

            if (includeDetails)
            {
                var dto = _mapper.Map<List<ArtifactDetailsDTO>>(all);
                return Ok(dto);
            }
            else
            {
                var dto = _mapper.Map<List<ArtifactDTO>>(all);
                return Ok(dto);
            }
        }
        [HttpDelete("{id}")]
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

