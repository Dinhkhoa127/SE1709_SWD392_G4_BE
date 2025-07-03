using AutoMapper;
using Azure;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.DTOs.Chapter;
using Microsoft.Extensions.Configuration;
using BiologyRecognition.DTOs.Recognition;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Net.Http;
using System.Text.Json;
using BiologyRecognition.Domain.Entities;

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
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IArtifactService _artifactService;

        public RecognitionController(IUserAccountService userAccountService, IMapper mapper, ISubjectService subjectService, IChapterService chapterService, IRecognitionService recognitionService, HttpClient httpClient, IConfiguration configuration, IArtifactService artifactService)
        {

            _accountService = userAccountService;
            _mapper = mapper;
            _subjectService = subjectService;
            _chapterService = chapterService;
            _recognitionService = recognitionService;
            _httpClient = httpClient;
            _configuration = configuration;
            _artifactService = artifactService;
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

        [HttpPost("recognize")]
        public async Task<IActionResult> Recognize([FromBody] ImageDTO imageDTO)
        {
            try
            {
                string imageUrl = imageDTO.ImageUrl;

                // 1. Tải ảnh từ Cloudinary với url được truyền vào
                var imageBytes = await _httpClient.GetByteArrayAsync(imageDTO.ImageUrl);
                var imageContent = new ByteArrayContent(imageBytes);

                // Kiểm tra và xác định Content-Type
                string fileExtension = Path.GetExtension(imageDTO.ImageUrl).ToLower(); // Lấy phần mở rộng từ URL
                string contentType;

                if (fileExtension == ".jpg" || fileExtension == ".jpeg")
                {
                    contentType = "image/jpeg";
                }
                else if (fileExtension == ".png")
                {
                    contentType = "image/png";
                }
                else
                {
                    throw new InvalidOperationException("Unsupported image format.");
                }

                // 2. Gửi ảnh sang DjangoAI (file multipart)
                string fileName = $"image{fileExtension}";
                var form = new MultipartFormDataContent();
                form.Add(imageContent, "image", fileName);

                string AIurl = _configuration["DjangoAI:DjangoURL"];

                var response = await _httpClient.PostAsync(AIurl, form);
                if (!response.IsSuccessStatusCode)
                    return BadRequest(new
                    {
                        message = "Error when calling AI service.",
                        statusCode = response.StatusCode,
                        reason = response.ReasonPhrase
                    }
                    );

                // read content rồi deserialize result thành DTO;
                var aiResultStr = await response.Content.ReadAsStringAsync();
                var aiResult = JsonSerializer.Deserialize<AIResultDTO>
                    (
                        aiResultStr,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                if (aiResult.Confidence < 0.7)
                {
                    var recogFailed = new Recognition();
                    recogFailed.UserId = imageDTO.UserId;
                    recogFailed.Artifact = null;
                    recogFailed.ArtifactId = null;
                    recogFailed.ImageUrl = imageDTO.ImageUrl;
                    recogFailed.RecognizedAt = DateTime.Now;
                    recogFailed.ConfidenceScore = aiResult.Confidence;
                    recogFailed.AiResult = "Không nhận dạng được chính xác";
                    recogFailed.Status = "Failed";
                    await _recognitionService.CreatAsync(recogFailed);
                    return BadRequest(new { success = false, message = "Không nhận dạng được." });
                }


                // 3. Lưu kết quả nhận dạng Recognition vào cơ sở dữ liệu và xử lý query DB artifact detail ở đây
                // ..... thầy nhân query theo yêu cầu của ae nha !!
                var artifacts = await _artifactService.GetArtifactsByContainsNameAsync(aiResult.ArtifactName);
                string message = "";
                if (artifacts != null && artifacts.Any())
                {
                    var firstArtifact = artifacts.First(); 

                    var recogSuccess = new Recognition
                    {
                        UserId = imageDTO.UserId,
                        ArtifactId = firstArtifact.ArtifactId,
                        ImageUrl = imageDTO.ImageUrl,
                        RecognizedAt = DateTime.Now,
                        ConfidenceScore = aiResult.Confidence,
                        AiResult = aiResult.ArtifactName,
                        Status = "Success"
                    };
                    var result = await _recognitionService.CreatAsync(recogSuccess);
                    message = result > 0 ? "Lưu thành công recognition" : "Lưu recognition thất bại";
                }
                // tạm thời trả về aiResult DTO
                return Ok(new { success = true, artifact = aiResult , message } );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi trong quá trình nhận diện.", error = ex.Message });
            }
        }
    }
}
