using AutoMapper;
using BiologyRecognition.Application;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Chapter;
using BiologyRecognition.DTOs.Topic;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [ApiController]
    [Route("api/chapter")]
    public class ChapterController : ControllerBase
    {

        private readonly IUserAccountService _accountService;
        private readonly IChapterService _chapterService;
        private readonly ISubjectService _subjectService;
        private readonly IMapper _mapper;

        public ChapterController(IUserAccountService userAccountService, IMapper mapper, ISubjectService subjectService,IChapterService chapterService)
        {

            _accountService = userAccountService;
            _mapper = mapper;
            _subjectService = subjectService;
            _chapterService = chapterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChapters()
        {
            var chapters = await _chapterService.GetAllAsync();
            if (chapters == null || chapters.Count == 0)
                return NotFound("Không tìm thấy bài nào.");

            var dto = _mapper.Map<List<ChapterDTO>>(chapters);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapterById(int id)
        {
            var chapter = await _chapterService.GetByIdAsync(id);
            if (chapter == null)
                return NotFound("Bài không tồn tại.");

            var dto = _mapper.Map<ChapterDTO>(chapter);
            return Ok(dto);
        }

        //[HttpGet("search")]
        //public async Task<IActionResult> GetChapterByName([FromQuery] string name)
        //{
        //    var chapter = await _chapterService.GetChapterByNameAsync(name);
        //    if (chapter == null)
        //        return NotFound("Không tìm thấy bài với tên đã nhập.");

        //    var dto = _mapper.Map<ChapterDTO>(chapter);
        //    return Ok(dto);
        //}

        [HttpGet("filter-name")]
        public async Task<IActionResult> GetChaptersByContainName([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống." });
            var list = await _chapterService.GetListChaptersByContainNameAsync(name);
            if (list == null || list.Count == 0)
                return NotFound("Không có bài phù hợp với từ khóa tìm kiếm.");

            var dto = _mapper.Map<List<ChapterDTO>>(list);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChapter([FromBody] CreateChapterDTO chapterDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }
            var subject = await _subjectService.GetSubjectByIdAsync(chapterDto.SubjectId);
            if (subject == null)
                return NotFound(new { message = "Không tìm thấy chương này trong hệ thống." });

            var account = await _accountService.GetUserAccountByIdAsync(chapterDto.CreatedBy);
            if (account == null)
                return NotFound(new { message = "Không tìm thấy người tạo trong hệ thống." });

            var chapter = _mapper.Map<Chapter>(chapterDto);
            var result = await _chapterService.CreateAsync(chapter);
            if (result > 0)
                return Ok(new { message = "Tạo bài thành công." });

            return BadRequest(new { message = "Tạo bài thất bại." });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChapter([FromBody] UpdateChapterDTO chapterDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }
            var subject = await _subjectService.GetSubjectByIdAsync(chapterDto.SubjectId);
            if (subject == null)
                return NotFound(new { message = "Không tìm thấy người chương này trong hệ thống." });
            var account = await _accountService.GetUserAccountByIdAsync(chapterDto.ModifiedBy ?? 0);
            if (account == null)
                return NotFound(new { message = "Không tìm thấy người sửa trong hệ thống." });

            var chapter = await _chapterService.GetByIdAsync(chapterDto.ChapterId);
            if (chapter == null)
                return NotFound(new { message = "Không tìm thấy bài." });

            _mapper.Map(chapterDto, chapter);
            var result = await _chapterService.UpdateAsync(chapter);
            if (result > 0)
                return Ok(new { message = "Cập nhật bài thành công." });

            return BadRequest(new { message = "Cập nhật bài thất bại." });
        }
        [HttpGet("by-subject/{subjectId}")]
        public async Task<IActionResult> GetTopicsByChapterId(int subjectId)
        {
            var chapters = await _chapterService.GetListChaptersBySubjectIdAsync(subjectId);

            if (chapters == null || chapters.Count == 0)
                return NotFound("Không có bài nào trong chương này.");

            var dto = _mapper.Map<List<ChapterDTO>>(chapters);
            return Ok(dto);
        }

    }
}
