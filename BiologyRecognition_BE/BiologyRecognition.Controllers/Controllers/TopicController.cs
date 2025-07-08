using AutoMapper;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Chapter;
using BiologyRecognition.DTOs.Topic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [ApiController]
    [Route("api/topic")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IChapterService _chapterService;
        private readonly IUserAccountService _accountService;
        private readonly IMapper _mapper;

        public TopicController(ITopicService topicService, IChapterService chapterService, IUserAccountService accountService, IMapper mapper)
        {
            _topicService = topicService;
            _chapterService = chapterService;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet(".")]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _topicService.GetAllAsync();
            if (topics == null || topics.Count == 0)
                return NotFound("Không tìm thấy chủ đề nào.");

            var dto = _mapper.Map<List<TopicDTO>>(topics);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopicById(int id)
        {
            var topic = await _topicService.GetByIdAsync(id);
            if (topic == null)
                return NotFound("Chủ đề không tồn tại.");

            var dto = _mapper.Map<TopicDTO>(topic);
            return Ok(dto);
        }

        [HttpGet("filter-name")]
        public async Task<IActionResult> GetTopicsByContainName([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống." });
            var list = await _topicService.GetListTopicsByContainNameAsync(name);
            if (list == null || list.Count == 0)
                return NotFound("Không có chủ đề phù hợp với từ khóa tìm kiếm.");

            var dto = _mapper.Map<List<TopicDTO>>(list);
            return Ok(dto);
        }
        [HttpGet("by-artifactName/{artifactName}")]
        public async Task<IActionResult> GetTopicsByArtifactName(string? artifactName)
        {
            if (string.IsNullOrWhiteSpace(artifactName))
                return BadRequest(new { message = "Tên đối tượng không được để trống." });

            var topics = await _topicService.GetListTopicsByArtifactNameAsync(artifactName);

            if (topics == null || topics.Count == 0)
                return NotFound("Không có chủ đề nào phù hợp với tên đối tượng.");

            var dto = topics.Select(topic =>
    _mapper.Map<TopicArtifactChapterDTO>(topic, opt =>
    {
        opt.Items["artifactName"] = artifactName.ToLower();
    })
).ToList();
             
            return Ok(dto);
        }

        [HttpGet("by-chapter/{chapterId}")]
        public async Task<IActionResult> GetTopicsByChapterId(int chapterId)
        {
            var topics = await _topicService.GetListTopicsByChapterIdAsync(chapterId);

            if (topics == null || topics.Count == 0)
                return NotFound("Không có nội dung nào trong bài này.");

            var dto = _mapper.Map<List<TopicDTO>>(topics);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicDTO topicDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            var chapter = await _chapterService.GetByIdAsync(topicDto.ChapterId);
            if (chapter == null)
                return NotFound(new { message = "Không tìm thấy chương trong hệ thống." });

            var account = await _accountService.GetUserAccountByIdAsync(topicDto.CreatedBy);
            if (account == null)
                return NotFound(new { message = "Không tìm thấy người tạo trong hệ thống." });

            var topic = _mapper.Map<Topic>(topicDto);
            var result = await _topicService.CreateAsync(topic);
            if (result > 0)
                return Ok(new { message = "Tạo chủ đề thành công." });

            return BadRequest(new { message = "Tạo chủ đề thất bại." });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromBody] UpdateTopicDTO topicDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            var chapter = await _chapterService.GetByIdAsync(topicDto.ChapterId);
            if (chapter == null)
                return NotFound(new { message = "Không tìm thấy chương trong hệ thống." });

            var account = await _accountService.GetUserAccountByIdAsync(topicDto.ModifiedBy ?? 0);
            if (account == null)
                return NotFound(new { message = "Không tìm thấy người sửa trong hệ thống." });

            var topic = await _topicService.GetByIdAsync(topicDto.TopicId);
            if (topic == null)
                return NotFound(new { message = "Không tìm thấy chủ đề." });

            _mapper.Map(topicDto, topic);
            var result = await _topicService.UpdateAsync(topic);
            if (result > 0)
                return Ok(new { message = "Cập nhật chủ đề thành công." });

            return BadRequest(new { message = "Cập nhật chủ đề thất bại." });
        }

        [HttpGet]
        public async Task<IActionResult> GetTopics(
    [FromQuery] int? id,
    [FromQuery] string? name,
    [FromQuery] string? artifactName,
    [FromQuery] int? chapterId
)
        {
            // 1. Nếu có ID -> Lấy theo ID
            if (id.HasValue)
            {
                var topic = await _topicService.GetByIdAsync(id.Value);
                if (topic == null)
                    return NotFound("Chủ đề không tồn tại.");

                var dto = _mapper.Map<TopicDTO>(topic);
                return Ok(dto);
            }

            // 2. Nếu có artifactName -> Lấy theo artifact
            if (!string.IsNullOrWhiteSpace(artifactName))
            {
                var topics = await _topicService.GetListTopicsByArtifactNameAsync(artifactName);
                if (topics == null || topics.Count == 0)
                    return NotFound("Không có chủ đề nào phù hợp với tên đối tượng.");

                var dto = topics.Select(topic =>
                    _mapper.Map<TopicArtifactChapterDTO>(topic, opt =>
                    {
                        opt.Items["artifactName"] = artifactName.ToLower();
                    })
                ).ToList();

                return Ok(dto);
            }

            // 3. Nếu có chapterId -> Lấy theo chương
            if (chapterId.HasValue)
            {
                var topics = await _topicService.GetListTopicsByChapterIdAsync(chapterId.Value);
                if (topics == null || topics.Count == 0)
                    return NotFound("Không có nội dung nào trong bài này.");

                var dto = _mapper.Map<List<TopicDTO>>(topics);
                return Ok(dto);
            }

            // 4. Nếu có name -> Tìm kiếm theo tên
            if (!string.IsNullOrWhiteSpace(name))
            {
                var list = await _topicService.GetListTopicsByContainNameAsync(name);
                if (list == null || list.Count == 0)
                    return NotFound("Không có chủ đề phù hợp với từ khóa tìm kiếm.");

                var dto = _mapper.Map<List<TopicDTO>>(list);
                return Ok(dto);
            }

            // 5. Nếu không có gì -> Trả toàn bộ
            var allTopics = await _topicService.GetAllAsync();
            if (allTopics == null || allTopics.Count == 0)
                return NotFound("Không tìm thấy chủ đề nào.");

            var allDto = _mapper.Map<List<TopicDTO>>(allTopics);
            return Ok(allDto);
        }

    }
}
