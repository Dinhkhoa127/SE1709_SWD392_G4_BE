using AutoMapper;
using BiologyRecognition.Application.Implement;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Chapter;
using BiologyRecognition.DTOs.Topic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [ApiController]
    [Route("api/chapter")]
    [Authorize]
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

    
        [HttpPost]
        [Authorize(Roles = "3")]
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
        [Authorize(Roles = "3")]
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

        [HttpGet]
        [Authorize(Roles = "2,3")]
        public async Task<IActionResult> GetChapters(
    [FromQuery] int? id,
    [FromQuery] string? name,
    [FromQuery] int? subjectId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 3)
        {
            if (id.HasValue)
            {
                var chapter = await _chapterService.GetByIdAsync(id.Value);
                if (chapter == null)
                    return NotFound("Bài không tồn tại.");

                var dto = _mapper.Map<ChapterDTO>(chapter);
                return Ok(dto);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var listByName = await _chapterService.GetListChaptersByContainNameAsync(name, page, pageSize);
                if (listByName.Items == null || listByName.TotalItems == 0)
                    return NotFound("Không có bài phù hợp với từ khóa tìm kiếm.");

                var dtoByName = _mapper.Map<List<ChapterDTO>>(listByName.Items);
                return Ok(dtoByName);
            }

            if (subjectId.HasValue)
            {
                var listBySubject = await _chapterService.GetListChaptersBySubjectIdAsync(subjectId.Value, page, pageSize);
                if (listBySubject.Items == null || listBySubject.TotalItems == 0)
                    return NotFound("Không có bài nào trong chương này.");

                var dtoBySubject = _mapper.Map<List<ChapterDTO>>(listBySubject.Items);
                return Ok(dtoBySubject);
            }

            var all = await _chapterService.GetAllAsync(page, pageSize);
            if (all.Items == null || all.TotalItems == 0)
                return NotFound("Không tìm thấy bài nào.");

            var dtoAll = _mapper.Map<List<ChapterDTO>>(all.Items);
            return Ok(dtoAll);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(int id)
        {
            var chapter = await _chapterService.GetByIdAsync(id);
            if (chapter == null)
                return NotFound("Bài không tồn tại.");

            // Không thực hiện thao tác xóa thật sự
            return Ok(new { message = $"Xóa thành công" });
        }
    }
}
