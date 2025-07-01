using AutoMapper;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Subject;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [ApiController]
    [Route("api/subject")]
    public class SubjectController : ControllerBase
    {
        private readonly IUserAccountService _accountService;
     
        private readonly ISubjectService _subjectService;
        private readonly IMapper _mapper;


        public SubjectController( IUserAccountService userAccountService, IMapper mapper, ISubjectService subjectService)
        {
            
            _accountService = userAccountService;
            _mapper = mapper;
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubjects()
        {
            var subjects = await _subjectService.GetAllAsync();
            if (subjects == null || subjects.Count == 0)
            {
                return NotFound("Không tìm thấy môn học nào.");
            }
            var dto = _mapper.Map<List<SubjectDTO>>(subjects);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null)
                return NotFound("Môn học không tồn tại.");

            var dto = _mapper.Map<SubjectDTO>(subject);
            return Ok(dto);
        }

        //[HttpGet("search")]
        //public async Task<IActionResult> GetSubjectByName([FromQuery] string name)
        //{
        //    var subject = await _subjectService.GetSubjectByNameAsync(name);
        //    if (subject == null)
        //        return NotFound("Không tìm thấy môn học với tên đã nhập.");

        //    var dto = _mapper.Map<SubjectDTO>(subject);
        //    return Ok(dto);
        //}
        //[HttpDelete("admin/{subjectId}")]
        //public async Task<IActionResult> DeleteSubjectByAdmin(int subjectId)
        //{
        //    // Kiểm tra subject tồn tại
        //    var subject = await _subjectService.GetSubjectByIdAsync(subjectId);
        //    if (subject == null)
        //        return NotFound(new { message = "Không tìm thấy môn học" });

        //    // Gọi service để xóa
        //    var result = await _subjectService.DeleteAsync(subject);
        //    if (result)
        //        return Ok(new { message = "Xóa môn học thành công" });

        //    return BadRequest(new { message = "Xóa môn học thất bại" });
        //}

        [HttpGet("filter-name")]
        public async Task<IActionResult> GetSubjectsByContainName([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống." });
            var list = await _subjectService.GetListSubjectByContainNameAsync(name);
            if (list == null || list.Count == 0)
                return NotFound("Không có môn học phù hợp với từ khóa tìm kiếm.");

            var dto = _mapper.Map<List<SubjectDTO>>(list);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDTO subjectDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }
            var account =await _accountService.GetUserAccountByIdAsync(subjectDto.CreatedBy);
            if (account == null)
                return NotFound(new { message = "Không tìm thấy người này trong hệ thống" });
            var subject = _mapper.Map<Subject>(subjectDto);
            var result = await _subjectService.CreateAsync(subject);
            if (result > 0)
                return Ok(new { message = "Tạo môn học thành công" });

            return BadRequest(new { message = "Tạo môn học thất bại" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectDTO subjectDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }


            var account = await _accountService.GetUserAccountByIdAsync(subjectDto.ModifiedBy);
            if (account == null)
                return NotFound(new { message = "Không tìm thấy người này trong hệ thống" });

            var subject = await _subjectService.GetSubjectByIdAsync(subjectDto.SubjectId);
            if (subject == null)
                return NotFound(new { message = "Không tìm thấy môn học" });

            _mapper.Map(subjectDto, subject);
            var result = await _subjectService.UpdateAsync(subject);
            if (result > 0)
                return Ok(new { message = "Cập nhật thành công" });

            return BadRequest(new { message = "Cập nhật thất bại" });
        }
    }
}
