using AutoMapper;
using BiologyRecognition.Application.Implement;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.ArtifactMedia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/artifactMedia")]
    [ApiController]
    [Authorize]
    public class ArtifactMediaController : ControllerBase
    {

        private readonly IArtifactMediaService _artifactMediaService;
        private readonly IArtifactService _artifactService;
        private readonly IMapper _mapper;

        public ArtifactMediaController(IArtifactMediaService artifactMediaService, IArtifactService artifactService, IMapper mapper)
        {
            _artifactMediaService = artifactMediaService;
            _artifactService = artifactService;
            _mapper = mapper;
        }
        [HttpPost]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Create([FromBody] CreateArtifactMediaDTO mediaDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            var artifact = await _artifactService.GetByIdAsync(mediaDto.ArtifactId);
            if (artifact == null)
                return NotFound(new { message = "Không tìm thấy Artifact liên kết." });


            var entity = _mapper.Map<ArtifactMedia>(mediaDto);
            var result = await _artifactMediaService.CreateAsync(entity);

            if (result > 0)
                return Ok(new { message = "Tạo ArtifactMedia thành công." });

            return BadRequest(new { message = "Tạo ArtifactMedia thất bại." });
        }

        [HttpPut]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Update([FromBody] UpdateArtifactMediaDTO mediaDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            var existing = await _artifactMediaService.GetByIdAsync(mediaDto.ArtifactMediaId);
            if (existing == null)
                return NotFound(new { message = "ArtifactMedia không tồn tại." });


            _mapper.Map(mediaDto, existing);
            var result = await _artifactMediaService.UpdateAsync(existing);

            if (result > 0)
                return Ok(new { message = "Cập nhật ArtifactMedia thành công." });

            return BadRequest(new { message = "Cập nhật ArtifactMedia thất bại." });
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> DeleteArtifactMedia(int id)
        {
            var media = await _artifactMediaService.GetByIdAsync(id);
            if (media == null)
                return NotFound(new { message = "Không tìm thấy ArtifactMedia để xóa." });

            var result = await _artifactMediaService.DeleteAsync(media);
            if (result)
                return Ok(new { message = "Xóa ArtifactMedia thành công." });

            return BadRequest(new { message = "Xóa ArtifactMedia thất bại." });
        }

        [HttpGet]
        [Authorize(Roles = "2,3")]
        public async Task<IActionResult> GetArtifactMedia(
    [FromQuery] int? id,
    [FromQuery] int? artifactId,
    [FromQuery] string? artifactName,
    [FromQuery] string? type,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 3)
        {
            //  1. Ưu tiên tìm theo ID
            if (id.HasValue)
            {
                var media = await _artifactMediaService.GetByIdAsync(id.Value);
                if (media == null)
                    return NotFound("ArtifactMedia không tồn tại.");

                var dto = _mapper.Map<ArtifactMediaDTO>(media);
                return Ok(dto);
            }

            //  2. Theo artifactId
            if (artifactId.HasValue)
            {
                var list = await _artifactMediaService.GetListArtifactMediaByArtifactIdAsync(artifactId.Value, page, pageSize);
                if (list.Items == null || list.TotalItems == 0)
                    return NotFound("Không tìm thấy media cho Artifact này.");

                var dto = _mapper.Map<List<ArtifactMediaDTO>>(list.Items);
                return Ok(dto);
            }

            //  3. Theo artifactName
            if (!string.IsNullOrWhiteSpace(artifactName))
            {
                var list = await _artifactMediaService.GetListArtifactMediaByArtifactNameAsync(artifactName, page, pageSize);
                if (list.Items == null || list.TotalItems == 0)
                    return NotFound("Không tìm thấy media cho sinh vật này.");

                var dto = _mapper.Map<List<ArtifactMediaDTO>>(list.Items);
                return Ok(dto);
            }

            //  4. Theo type
            if (!string.IsNullOrWhiteSpace(type))
            {
                var validTypes = new[] { "IMAGE", "VIDEO", "AUDIO", "DOCUMENT" };
                if (!validTypes.Contains(type.ToUpper()))
                {
                    return BadRequest(new { message = "Loại media phải là IMAGE, VIDEO, AUDIO hoặc DOCUMENT." });
                }

                var list = await _artifactMediaService.GetListArtifactMediaByTypeAsync(type.ToUpper(),page,pageSize);
                if (list.Items == null || list.TotalItems == 0)
                    return NotFound("Không có media nào với kiểu này.");

                var dto = _mapper.Map<List<ArtifactMediaDTO>>(list.Items);
                return Ok(dto);
            }

            // 5. Nếu không truyền gì → trả về tất cả
            var all = await _artifactMediaService.GetAllAsync(page,pageSize);
            if (all.Items == null || all.TotalItems == 0)
                return NotFound("Không tìm thấy ArtifactMedia nào.");

            var allDto = _mapper.Map<List<ArtifactMediaDTO>>(all.Items);
            return Ok(allDto);
        }

    }
}
