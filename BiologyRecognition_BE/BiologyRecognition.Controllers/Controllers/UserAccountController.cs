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
using BiologyRecognition.Application.Implement;

namespace BiologyRecognition.Controllers.Controllers
{
    [Route("api/user-accounts")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetAccounts([FromQuery] int? id, [FromQuery] int page = 1,
    [FromQuery] int pageSize = 3)
        {
            if (id.HasValue)
            {
                var account = await _accountService.GetUserAccountByIdAsync(id.Value);
                if (account == null)
                    return NotFound("Tài khoản không tồn tại.");

                var dto = _mapper.Map<UserAccountDTO>(account);
                return Ok(dto);
            }

            var accounts = await _accountService.GetAllAsync(page,pageSize);
            if (accounts.Items == null || accounts.TotalItems == 0)
                return NotFound("Không tìm thấy tài khoản nào.");

            var dtoList = _mapper.Map<List<UserAccountDTO>>(accounts.Items);
            return Ok(dtoList);
        }

        [HttpPost]
        [Authorize(Roles ="1")]
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


        [HttpPut("me/info")]
        [Authorize(Roles = "2,3")]

        public async Task<IActionResult> UpdateAccountInfo([FromBody] UpdateAccountStudentNoPwDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }
            var currentUserIdClaim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim) || dto.UserAccountId.ToString() != currentUserIdClaim)
            {
                return Unauthorized(new { message = "You are not allowed to update another account's password." });
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

        [HttpPut("me/password")]
        [Authorize(Roles = "2,3")]

        public async Task<IActionResult> UpdateStudentPassword([FromBody] UpdateAccountStudentPwDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }
            var currentUserIdClaim = User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(currentUserIdClaim) || dto.UserAccountId.ToString() != currentUserIdClaim)
            {
                return Unauthorized(new { message = "You are not allowed to update another account's password." });
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
        [Authorize(Roles = "1")]

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

       

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _accountService.GetUserAccountByIdAsync(id);
            if (account == null)
                return NotFound("Tài khoản không tồn tại.");

            // Không thực hiện thao tác xóa thật sự
            return Ok(new { message = $"Xóa thành công" });
        }
    }
}
