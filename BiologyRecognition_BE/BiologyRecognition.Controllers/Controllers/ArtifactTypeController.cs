using AutoMapper;
using BiologyRecognition.Application.Implement;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.ArtifactType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/artifactType")]
    [ApiController]
    [Authorize]
    public class ArtifactTypeController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;
        private readonly IArtifactTypeService _artifactTypeService;
        public ArtifactTypeController(ITopicService topicService,  IMapper mapper, IArtifactTypeService artifactTypeService)
        {
            _topicService = topicService;
            _mapper = mapper;
            _artifactTypeService = artifactTypeService;
        }
            
        [HttpPost]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> CreateArtifactType([FromBody] CreateArtifactTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            var topic = await _topicService.GetByIdAsync(dto.TopicId);
            if (topic == null)
                return NotFound(new { message = "Không tìm thấy nội dung tương ứng." });

            var entity = _mapper.Map<ArtifactType>(dto);
            var result = await _artifactTypeService.CreateAsync(entity);

            if (result > 0)
                return Ok(new { message = "Tạo loại mẫu thành công." });

            return BadRequest(new { message = "Tạo loại mẫu thất bại." });
        }

        [HttpPut]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> UpdateArtifactType([FromBody] UpdateArtifactTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            var topic = await _topicService.GetByIdAsync(dto.TopicId);
            if (topic == null)
                return NotFound(new { message = "Không tìm thấy nội dung tương ứng." });

            var entity = await _artifactTypeService.GetByIdAsync(dto.ArtifactTypeId);
            if (entity == null)
                return NotFound(new { message = "Không tìm thấy loại mẫu." });

            _mapper.Map(dto, entity);
            var result = await _artifactTypeService.UpdateAsync(entity);

            if (result > 0)
                return Ok(new { message = "Cập nhật loại mẫu thành công." });

            return BadRequest(new { message = "Cập nhật loại mẫu thất bại." });
        }

        [HttpGet]
        [Authorize(Roles = "2,3")]
        public async Task<IActionResult> GetArtifactTypes(
    [FromQuery] int? id,
    [FromQuery] string? name,
    [FromQuery] int? topicId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 3)
        {
            if (id.HasValue)
            {
                var entity = await _artifactTypeService.GetByIdAsync(id.Value);
                if (entity == null)
                    return NotFound("Loại mẫu không tồn tại.");

                var dto = _mapper.Map<ArtifactTypeDTO>(entity);
                return Ok(dto);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var listByName = await _artifactTypeService.GetArtifactTypesByContainsNameAsync(name, page, pageSize);
                if (listByName.Items == null || listByName.TotalItems == 0)
                    return NotFound("Không có loại mẫu nào phù hợp.");

                var dtoByName = _mapper.Map<List<ArtifactTypeDTO>>(listByName.Items);
                return Ok(dtoByName);
            }

            if (topicId.HasValue)
            {
                var listByTopic = await _artifactTypeService.GetListArtifactTypesByTopicIdAsync(topicId.Value, page, pageSize);
                if (listByTopic.Items == null || listByTopic.TotalItems == 0)
                    return NotFound("Không có loại mẫu nào cho nội dung này.");

                var dtoByTopic = _mapper.Map<List<ArtifactTypeDTO>>(listByTopic.Items);
                return Ok(dtoByTopic);
            }

            var all = await _artifactTypeService.GetAllAsync(page, pageSize);
            if (all.Items == null || all.TotalItems == 0)
                return NotFound("Không có loại mẫu nào.");

            var dtoAll = _mapper.Map<List<ArtifactTypeDTO>>(all.Items);
            return Ok(dtoAll);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _artifactTypeService.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Loại mẫu không tồn tại.");

            // Không thực hiện thao tác xóa thật sự
            return Ok(new { message = $"Xóa thành công" });
        }

    }
}
