using AutoMapper;
using AutoMapper.Execution;
using BiologyRecognition.Application;
using BiologyRecognition.DTOs.UserAccount;
using BiologyRecognition.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace BiologyRecognition.Controllers.Controllers
{
    [Route("api/user-accounts")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _accountService;
        private readonly IMapper _mapper;


        public UserAccountController( IUserAccountService userAccountService, IMapper mapper)
        {
           
            _accountService = userAccountService;
            _mapper = mapper;
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
            var dto = _mapper.Map<List<UserAccountDTO >>(accounts);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var account = await _accountService.GetUserAccountByIdAsync(id);
            if (account == null)
                return NotFound ("Tài khoản không tồn tại." );

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
                return Ok(new { message = "Tạo tài khoản thành công" });

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
    }
}
