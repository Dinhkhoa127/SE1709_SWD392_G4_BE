using BiologyRecognition.DTOs.UserAccount;
using BiologyAuthService = BiologyRecognition.Application.Interface.IAuthenticationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BiologyRecognition.Application.Interface;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AuthenticationController : ControllerBase
    {
        private readonly BiologyAuthService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly IUserAccountService _accountService;
        public AuthenticationController(BiologyAuthService authenticationService, IEmailService emailService, IUserAccountService userAccountService)
        {
            _authenticationService = authenticationService;
            _emailService = emailService;
            _accountService = userAccountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var account = await _authenticationService.Login(loginDTO);

                // Gắn access token vào cookie
                Response.Cookies.Append("access_token", account.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Bật nếu dùng HTTPS
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(3) // Hoặc theo cấu hình
                });

                return Ok(account);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                var account = await _authenticationService.Register(registerDTO);
                await _emailService.SendAccountRegisterEmailAsync(registerDTO.UserName, registerDTO.Email,registerDTO.FullName,DateTime.Now);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleCallback", "Authentication");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!result.Succeeded)
                {
                    return Unauthorized("Google login failed.");
                }

                var email = result.Principal.FindFirstValue(ClaimTypes.Email);
                var name = result.Principal.FindFirstValue(ClaimTypes.Name);
                var picture = result.Principal.FindFirstValue("picture");

                //if (string.IsNullOrEmpty(email))
                //    return Unauthorized("Login failed. Email not found");

                var user = await _authenticationService.LoginWithGoolgle(email, name);
                var redirectUrl = $"https://se-1709-swd-392-g4-fe.vercel.app/?accessToken={user.AccessToken}"; // hardcode FE, sau này sẽ thay bằng config
                // Gắn access token vào cookie
                Response.Cookies.Append("access_token", user.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Bật nếu dùng HTTPS
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(3) // Hoặc theo cấu hình
                });
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var currentUser = await _authenticationService.GetCurrentUser();
                return Ok(currentUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("otp/send")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOtp([FromBody] EmailDTO email)
        {
            if (string.IsNullOrWhiteSpace(email.Email))
                return BadRequest(new { message = "Email không được để trống." });

            var user = await _accountService.GetUserAccountByNameOrEmailAsync(email.Email);

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

        [HttpPost("otp/verify")]
        [AllowAnonymous]
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

        [HttpPost("otp/reset-password")]
        [AllowAnonymous]
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
