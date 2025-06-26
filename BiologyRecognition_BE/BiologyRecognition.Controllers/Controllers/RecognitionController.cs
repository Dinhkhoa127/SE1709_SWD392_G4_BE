using AutoMapper;
using BiologyRecognition.Application;
using BiologyRecognition.DTOs.Chapter;
using BiologyRecognition.DTOs.Recognition;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/recognition")]
    [ApiController]
    public class RecognitionController : ControllerBase
    {
      
        private readonly IUserAccountService _accountService;
        private readonly IChapterService _chapterService;
        private readonly ISubjectService _subjectService;
        private readonly IRecognitionService _recognitionService;
        private readonly IMapper _mapper;

        public RecognitionController(IUserAccountService userAccountService, IMapper mapper, ISubjectService subjectService, IChapterService chapterService, IRecognitionService recognitionService            )
        {

            _accountService = userAccountService;
            _mapper = mapper;
            _subjectService = subjectService;
            _chapterService = chapterService;
            _recognitionService = recognitionService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllRecognitions()
        {
            var recognitions = await _recognitionService.GetAllAsync();
            if (recognitions == null || recognitions.Count == 0)
                return NotFound("Không tìm thấy recognition nào.");

            var dto = _mapper.Map<List<RecognitionDTO>>(recognitions);
            return Ok(dto);
        }

  
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecognitionById(int id)
        {
            var recognition = await _recognitionService.GetByIdAsync(id);
            if (recognition == null || recognition.RecognitionId == 0)
                return NotFound("Không tìm thấy lịch sử tồn tại.");

            var dto = _mapper.Map<RecognitionDTO>(recognition);
            return Ok(dto);
        }

  
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRecognitionsByUserId(int userId)
        {
            var account = await _accountService.GetUserAccountByIdAsync(userId);
            if (account == null)
                return NotFound(new { message = "Không tìm thấy người này trong hệ thống." });
            var recognitions = await _recognitionService.GetRecognitionUserByIdAsync(userId);
            if (recognitions == null || recognitions.Count == 0)
                return NotFound("Người dùng này chưa có lịch sử nào.");

            var dto = _mapper.Map<List<RecognitionDTO>>(recognitions);
            return Ok(dto);
        }
    }
}
