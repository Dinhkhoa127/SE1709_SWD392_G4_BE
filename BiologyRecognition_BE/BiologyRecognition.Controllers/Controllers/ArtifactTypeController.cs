using AutoMapper;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.ArtifactType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/artifactType")]
    [ApiController]
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
        [HttpGet]
        public async Task<IActionResult> GetAllArtifactTypes()
        {
            var list = await _artifactTypeService.GetAllAsync();
            if (list == null || list.Count == 0)
                return NotFound("Không có loại mẫu nào.");

            var dto = _mapper.Map<List<ArtifactTypeDTO>>(list);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtifactTypeById(int id)
        {
            var entity = await _artifactTypeService.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Loại mẫu không tồn tại.");

            var dto = _mapper.Map<ArtifactTypeDTO>(entity);
            return Ok(dto);
        }

        [HttpGet("filter-name")]
        public async Task<IActionResult> GetArtifactTypesByContainName([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống." });

            var list = await _artifactTypeService.GetArtifactTypesByContainsNameAsync(name);
            if (list == null || list.Count == 0)
                return NotFound("Không có loại mẫu nào phù hợp.");

            var dto = _mapper.Map<List<ArtifactTypeDTO>>(list);
            return Ok(dto);
        }

        [HttpPost]
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

        [HttpGet("by-topic/{topicId}")]
        public async Task<IActionResult> GetArtifactTypesByTopicId(int topicId)
        {
            var list = await _artifactTypeService.GetListArtifactTypesByTopicIdAsync(topicId);
            if (list == null || list.Count == 0)
                return NotFound("Không có loại mẫu nào cho nội dung này.");

            var dto = _mapper.Map<List<ArtifactTypeDTO>>(list);
            return Ok(dto);
        }
    }
}
