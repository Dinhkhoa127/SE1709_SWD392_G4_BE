using AutoMapper;
using AutoMapper.Execution;
using BiologyRecognition.Application;
using BiologyRecognition.Controllers.Models;
using BiologyRecognition.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace BiologyRecognition.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _accountService;
        private readonly ILogger<UserAccountController> _logger;
        private readonly IMapper _mapper;

        public UserAccountController(ILogger<UserAccountController> logger, IUserAccountService userAccountService, IMapper mapper)
        {
            _logger = logger;
            _accountService = userAccountService;
            _mapper = mapper;
        }

        [HttpGet("GetAllAccount")]
        public async Task<IActionResult> GetAllAccountInfo()
        {
            var accounts = await _accountService.GetAllAsync();
            if (accounts == null)
            {
                return NotFound("Không tìm thấy tài khoản nào.");
            }
            var dto = _mapper.Map <List<UserAccountDTO >>(accounts);
            return Ok(dto);
        }

        [HttpGet("GetAccountBy{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var account = await _accountService.GetUserAccountByIdAsync(id);
            if (account == null)
                return NotFound ("Tài khoản không tồn tại." );

            var dto = _mapper.Map<UserAccountDTO>(account);
            return Ok(dto);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginAccount)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { message = errors });
            }

            var account = await _accountService.GetUserAccountByNameOrEmailAsync(loginAccount.UserNameOrEmail);
            if (account == null || account.Password != loginAccount.Password)
            {
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu.");
            }

            if (account.IsActive != true)
            {
                return Unauthorized("Tài khoản không hoạt động");
            }
            var dto = _mapper.Map<UserAccountDTO>(account);
            return Ok(dto);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO newAccount)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { message = errors });
            }
            newAccount.Email = newAccount.Email?.Trim();
            newAccount.Phone = newAccount.Phone?.Trim();
            if (!Regex.IsMatch(newAccount.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest(new { message = "Email không đúng định dạng" });
            if (await _accountService.GetUserAccountByNameOrEmailAsync(newAccount.UserName) != null)
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại" });

            if (await _accountService.GetUserAccountByNameOrEmailAsync(newAccount.Email) != null)
                return BadRequest(new { message = "Email đã tồn tại" });
            var phoneExists = await _accountService.GetUserAccountByPhone(newAccount.Phone.Trim());
            if (phoneExists != null)
            {
                return BadRequest(new { message = "Số điện thoại đã được sử dụng" });
            }
            var userAccount = _mapper.Map<UserAccount>(newAccount);

            var result = await _accountService.CreateAsync(userAccount);
            if (result > 0)
            {
                return Ok(new { message = "Tạo tài khoản thành công" });
            }
            else
            {
                return BadRequest(new { message = "Tạo tài khoản thất bại" });
            }
        }

        [HttpPut("Update-student")]
        public async Task<IActionResult> UpdateAccountByStudent([FromBody] UpdateAccountStudentDTO updateAccountStudent)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = errors });
            }

            updateAccountStudent.Email = updateAccountStudent.Email?.Trim();
            updateAccountStudent.Phone = updateAccountStudent.Phone?.Trim();

            var user = await _accountService.GetUserAccountByIdAsync(updateAccountStudent.UserAccountId);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản" });

            var emailExists = await _accountService.GetUserAccountByNameOrEmailAsync(updateAccountStudent.Email);
            if (emailExists !=null)
                return BadRequest(new { message = "Email đã được sử dụng" });

            var phoneExists = await _accountService.GetUserAccountByPhone(updateAccountStudent.Phone.Trim());
            if (phoneExists != null && phoneExists.UserAccountId != updateAccountStudent.UserAccountId)
            {
                return BadRequest(new { message = "Số điện thoại đã được sử dụng" });
            }


            _mapper.Map(updateAccountStudent, user);

            var result = await _accountService.UpdateAsync(user);
            if (result > 0)
                return Ok(new { message = "Cập nhật thành công" });

            return BadRequest(new { message = "Cập nhật thất bại" });
        }

        [HttpPut("Update-admin")]
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
            if (emailExists != null)
                return BadRequest(new { message = "Email đã được sử dụng" });

            var phoneExists = await _accountService.GetUserAccountByPhone(updateAccountAdmin.Phone.Trim());
            if (phoneExists != null && phoneExists.UserAccountId != updateAccountAdmin.UserAccountId)
            {
                return BadRequest(new { message = "Số điện thoại đã được sử dụng" });
            }


            _mapper.Map(updateAccountAdmin, user);

            var result = await _accountService.UpdateAsync(user);
            if (result > 0)
                return Ok(new { message = "Cập nhật thành công" });

            return BadRequest(new { message = "Cập nhật thất bại" });
        }
    }
}
