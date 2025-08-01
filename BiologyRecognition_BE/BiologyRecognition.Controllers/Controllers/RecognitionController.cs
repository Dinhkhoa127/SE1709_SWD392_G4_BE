﻿using AutoMapper;
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
using Microsoft.AspNetCore.Authorization;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/recognition")]
    [ApiController]
    [Authorize]
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

        [HttpPost("recognize")]
        [Authorize(Roles = "2")]
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
                string message = "";
                if (aiResult.Confidence < 0.5)
                {
                    var recogFailed = await _recognitionService.CreateFailedRecognition(imageDTO, aiResult.Confidence);
                    var result = await _recognitionService.CreatAsync(recogFailed);
                    message = result > 0 ? "Lưu thành công recognition. Không nhận dạng được thực thể" : "Lưu recognition thất bại. Không nhận dạng được thực thể";
                    return BadRequest(new { success = false, message });
                }


                // 3. Lưu kết quả nhận dạng Recognition vào cơ sở dữ liệu và xử lý query DB artifact detail ở đây
                var artifacts = await _artifactService.GetArtifactsByContainsNameAsync(aiResult.ArtifactName);
                
                if (artifacts != null && artifacts.Any())
                {
                    var firstArtifact = artifacts.First(); 
                    var recogSuccess =  await _recognitionService.CreateSuccessRecognition(imageDTO, firstArtifact, aiResult.Confidence);
                    var result = await _recognitionService.CreatAsync(recogSuccess);
                    message = result > 0 ? "Lưu thành công recognition. Đã nhận dạng được thực thể" : "Lưu recognition thất bại. Đã nhận dạng được thực thể";
                }
                // tạm thời trả về aiResult DTO
                return Ok(new { success = true, artifact = aiResult , message } );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi trong quá trình nhận diện.", error = ex.Message, inner = ex.InnerException?.Message });
            }
        }
        [HttpGet]
        [Authorize(Roles = "2,3")]
        public async Task<IActionResult> GetRecognitions(
    [FromQuery] int? id,
    [FromQuery] int? userId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 3)
        {
            // Lấy theo ID recognition
            if (id.HasValue)
            {
                var recognition = await _recognitionService.GetByIdAsync(id.Value);
                if (recognition == null || recognition.RecognitionId == 0)
                    return NotFound("Không tìm thấy lịch sử tồn tại.");

                var dto = _mapper.Map<RecognitionDTO>(recognition);
                return Ok(dto);
            }

            // Lấy theo ID người dùng
            if (userId.HasValue)
            {
                var account = await _accountService.GetUserAccountByIdAsync(userId.Value);
                if (account == null)
                    return NotFound(new { message = "Không tìm thấy người này trong hệ thống." });

                var recognitions = await _recognitionService.GetRecognitionUserByIdAsync(userId.Value, page, pageSize);
                if (recognitions.Items == null || recognitions.TotalItems == 0)
                    return NotFound("Người dùng này chưa có lịch sử nào.");

                var dto = _mapper.Map<List<RecognitionDTO>>(recognitions.Items);
                return Ok(dto);
            }

            // Lấy tất cả nếu không truyền gì
            var all = await _recognitionService.GetAllAsync(page, pageSize);
            if (all.Items == null || all.TotalItems == 0)
                return NotFound("Không tìm thấy recognition nào.");

            var dtoAll = _mapper.Map<List<RecognitionDTO>>(all.Items);
            return Ok(dtoAll);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(int id)
        {
            var recognition = await _recognitionService.GetByIdAsync(id);
            if (recognition == null || recognition.RecognitionId == 0)
                return NotFound("Không tìm thấy lịch sử tồn tại.");

            // Không thực hiện thao tác xóa thật sự
            return Ok(new { message = $"Xóa thành công" });
        }
    }
}
