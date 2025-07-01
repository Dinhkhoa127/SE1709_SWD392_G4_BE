using AutoMapper;
using AutoMapper.Execution;
using BiologyRecognition.DTOs.UserAccount;
using BiologyRecognition.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using BiologyRecognition.Application.Interface;
using Microsoft.EntityFrameworkCore;

namespace BiologyRecognition.Controllers.Controllers
{
    [Route("api/user-accounts")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;


        public UserAccountController(IUserAccountService userAccountService, IMapper mapper, IEmailService emailService)
        {

            _accountService = userAccountService;
            _mapper = mapper;
            _emailService = emailService;
        }


        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllAccountInfo()
        {
            var accounts = await _accountService.GetAllAsync();
            if (accounts == null)
            {
                return NotFound("Không tìm thấy tài khoản nào.");
            }
            var dto = _mapper.Map<List<UserAccountDTO>>(accounts);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var account = await _accountService.GetUserAccountByIdAsync(id);
            if (account == null)
                return NotFound("Tài khoản không tồn tại.");

            var dto = _mapper.Map<UserAccountDTO>(account);
            return Ok(dto);
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAccountByAdmin([FromBody] CreateAccountDTO createAccountDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            // Làm sạch chuỗi
            createAccountDto.Email = createAccountDto.Email?.Trim();
            createAccountDto.Phone = createAccountDto.Phone?.Trim();

            // Kiểm tra trùng Email
            var emailExists = await _accountService.GetUserAccountByNameOrEmailAsync(createAccountDto.Email);
            if (emailExists != null)
                return BadRequest(new { message = "Email đã được sử dụng" });

            // Kiểm tra trùng số điện thoại
            var phoneExists = await _accountService.GetUserAccountByPhone(createAccountDto.Phone);
            if (phoneExists != null)
                return BadRequest(new { message = "Số điện thoại đã được sử dụng" });

            // Map sang entity UserAccount
            var userAccount = _mapper.Map<UserAccount>(createAccountDto);

            // Gọi service để lưu
            var result = await _accountService.CreateAccountByAdminAsync(userAccount);
            if (result > 0)
            {
                await _emailService.SendAccountCreationEmailAsync(userAccount.UserName, userAccount.Email, "123@");
                return Ok(new { message = "Tạo tài khoản thành công" });
            }

            return BadRequest(new { message = "Tạo tài khoản thất bại" });
        }


        [HttpPut("student/update-info")]
        public async Task<IActionResult> UpdateAccountInfo([FromBody] UpdateAccountStudentNoPwDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            dto.Email = dto.Email?.Trim();
            dto.Phone = dto.Phone?.Trim();

            var user = await _accountService.GetUserAccountByIdAsync(dto.UserAccountId);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản" });

            var emailExists = await _accountService.GetUserAccountByNameOrEmailAsync(dto.Email);
            if (emailExists != null && emailExists.UserAccountId != dto.UserAccountId)
                return BadRequest(new { message = "Email đã được sử dụng" });

            var phoneExists = await _accountService.GetUserAccountByPhone(dto.Phone);
            if (phoneExists != null && phoneExists.UserAccountId != dto.UserAccountId)
                return BadRequest(new { message = "Số điện thoại đã được sử dụng" });

            _mapper.Map(dto, user);
            var result = await _accountService.UpdateAsync(user);
            if (result > 0)
                return Ok(new { message = "Cập nhật thông tin thành công" });

            return BadRequest(new { message = "Cập nhật thất bại" });
        }

        [HttpPut("student/update-password")]
        public async Task<IActionResult> UpdateStudentPassword([FromBody] UpdateAccountStudentPwDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            var user = await _accountService.GetUserAccountByIdAsync(dto.UserAccountId);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản" });

            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            _mapper.Map(dto, user);
            var result = await _accountService.UpdateAsync(user);

            if (result > 0)
                return Ok(new { message = "Cập nhật mật khẩu thành công" });

            return BadRequest(new { message = "Cập nhật mật khẩu thất bại" });
        }


        [HttpPut("admin")]
        public async Task<IActionResult> UpdateAccountByAdmin([FromBody] UpdateAccountAdminDTO updateAccountAdmin)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            updateAccountAdmin.Email = updateAccountAdmin.Email?.Trim();
            updateAccountAdmin.Phone = updateAccountAdmin.Phone?.Trim();

            var user = await _accountService.GetUserAccountByIdAsync(updateAccountAdmin.UserAccountId);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản" });

            var emailExists = await _accountService.GetUserAccountByNameOrEmailAsync(updateAccountAdmin.Email);
            if (emailExists != null && emailExists.UserAccountId != updateAccountAdmin.UserAccountId)
                return BadRequest(new { message = "Email đã được sử dụng" });

            var phoneExists = await _accountService.GetUserAccountByPhone(updateAccountAdmin.Phone.Trim());
            if (phoneExists != null && phoneExists.UserAccountId != updateAccountAdmin.UserAccountId)
            {
                return BadRequest(new { message = "Số điện thoại đã được sử dụng" });
            }

            updateAccountAdmin.Password = BCrypt.Net.BCrypt.HashPassword(updateAccountAdmin.Password);
            _mapper.Map(updateAccountAdmin, user);

            var result = await _accountService.UpdateAsync(user);
            if (result > 0)
                return Ok(new { message = "Cập nhật thành công" });

            return BadRequest(new { message = "Cập nhật thất bại" });
        }

        [HttpPost("send-otp/{email}")]
        public async Task<IActionResult> SendOtp(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { message = "Email không được để trống." });

            var user = await _accountService.GetUserAccountByNameOrEmailAsync(email);

            // Trả lời chung dù email không tồn tại (chống dò)
            if (user == null)
                return Ok("Nếu email tồn tại, mã OTP đã được gửi.");

            // Nếu đã có OTP chưa hết hạn thì không cho gửi tiếp
            if (!string.IsNullOrEmpty(user.OtpCode) && user.OtpExpiry > DateTime.Now)
                return BadRequest("Bạn đã yêu cầu mã OTP gần đây. Vui lòng đợi vài phút.");

            var otp = new Random().Next(100000, 999999).ToString();

            // Mã hóa OTP trước khi lưu (optional nhưng nên)
            user.OtpCode = BCrypt.Net.BCrypt.HashPassword(otp);
            user.OtpExpiry = DateTime.Now.AddMinutes(5); // OTP có hạn 5 phút

            await _accountService.UpdateAsync(user);

            var body = $@"
        <p>Chào {user.FullName},</p>
        <p>Mã OTP để đặt lại mật khẩu là: <b>{otp}</b></p>
        <p>Mã này có hiệu lực trong 5 phút.</p>";

            await _emailService.SendEmailAsync(user.Email, "[BRS] Mã OTP đặt lại mật khẩu", body);

            return Ok("Nếu email tồn tại, mã OTP đã được gửi.");
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.OtpCode))
                return BadRequest("Email và mã OTP không được để trống.");

            var user = await _accountService.GetUserAccountByNameOrEmailAsync(req.Email);
            if (user == null || string.IsNullOrEmpty(user.OtpCode) || user.OtpExpiry < DateTime.Now)
                return BadRequest("Mã OTP không đúng hoặc đã hết hạn.");

            // So sánh OTP đã mã hóa
            if (!BCrypt.Net.BCrypt.Verify(req.OtpCode.Trim(), user.OtpCode))
                return BadRequest("Mã OTP không đúng.");

            return Ok("Xác minh thành công.");
        }

        [HttpPost("reset-password-with-otp")]
        public async Task<IActionResult> ResetPasswordWithOtp([FromBody] ResetByOtpRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) ||
                string.IsNullOrWhiteSpace(req.OtpCode) ||
                string.IsNullOrWhiteSpace(req.NewPassword))
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");
            }

            var user = await _accountService.GetUserAccountByNameOrEmailAsync(req.Email);

            if (user == null || string.IsNullOrEmpty(user.OtpCode) || user.OtpExpiry < DateTime.Now)
                return BadRequest("Mã OTP không đúng hoặc đã hết hạn.");

            if (!BCrypt.Net.BCrypt.Verify(req.OtpCode.Trim(), user.OtpCode))
                return BadRequest("Mã OTP không đúng.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            user.OtpCode = null;
            user.OtpExpiry = null;
            user.ModifiedDate = DateTime.Now;

            await _accountService.UpdateAsync(user);


            return Ok("Mật khẩu đã được đặt lại.");
        }

    }
}
