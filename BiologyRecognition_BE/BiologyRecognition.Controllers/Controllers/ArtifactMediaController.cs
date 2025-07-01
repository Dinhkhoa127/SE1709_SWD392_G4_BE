using AutoMapper;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.ArtifactMedia;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/artifactMedia")]
    [ApiController]
    public class ArtifactMediaController : ControllerBase
    {

        private readonly IArtifactMediaService _artifactMediaService;
        private readonly IArtifactService _artifactService;
        private readonly IMapper _mapper;

        public ArtifactMediaController(IArtifactMediaService artifactMediaService,IArtifactService artifactService, IMapper mapper)
        {
            _artifactMediaService = artifactMediaService;
            _artifactService = artifactService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArtifactMedia()
        {
            var list = await _artifactMediaService.GetAllAsync();
            if (list == null || list.Count == 0)
                return NotFound("Không tìm thấy ArtifactMedia nào.");

            var dto = _mapper.Map<List<ArtifactMediaDTO>>(list);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtifactMediaById(int id)
        {
            var media = await _artifactMediaService.GetByIdAsync(id);
            if (media == null)
                return NotFound("ArtifactMedia không tồn tại.");

            var dto = _mapper.Map<ArtifactMediaDTO>(media);
            return Ok(dto);
        }

        [HttpGet("by-artifact/{artifactId}")]
        public async Task<IActionResult> GetArtifactMediaByArtifactId(int artifactId)
        {
            var list = await _artifactMediaService.GetListArtifactMediaByArtifactIdAsync(artifactId);
            if (list == null || list.Count == 0)
                return NotFound("Không tìm thấy media cho Artifact này.");

            var dto = _mapper.Map<List<ArtifactMediaDTO>>(list);
            return Ok(dto);
        }
        [HttpGet("by-artifactName/{artifactName}")]
        public async Task<IActionResult> GetArtifactMediaByArtifactName(string? artifactName)
        {
            if (string.IsNullOrWhiteSpace(artifactName))
                return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống." });
            var list = await _artifactMediaService.GetListArtifactMediaByArtifactNameAsync(artifactName);
            if (list == null || list.Count == 0)
                return NotFound("Không tìm thấy media cho sinh vật này.");

            var dto = _mapper.Map<List<ArtifactMediaDTO>>(list);
            return Ok(dto);
        }
        [HttpGet("by-type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var validTypes = new[] { "IMAGE", "VIDEO", "AUDIO", "DOCUMENT" };
            if (!validTypes.Contains(type.ToUpper()))
            {
                return BadRequest(new { message = "Loại media phải là IMAGE, VIDEO, AUDIO hoặc DOCUMENT." });
            }

            var list = await _artifactMediaService.GetListArtifactMediaByTypeAsync(type.ToUpper());
            if (list == null || list.Count == 0)
                return NotFound("Không có media nào với kiểu này.");

            var dto = _mapper.Map<List<ArtifactMediaDTO>>(list);
            return Ok(dto);
        }

        [HttpPost]
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
    }
}
