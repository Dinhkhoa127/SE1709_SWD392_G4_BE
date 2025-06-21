using BiologyRecognition.Application;
using BiologyRecognition.DTOs.UserAccount;
using BiologyAuthService = BiologyRecognition.Application.IAuthenticationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BiologyRecognition.Controller.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly BiologyAuthService _authenticationService;

        public AuthenticationController(BiologyAuthService authenticationService)
        {
            _authenticationService = authenticationService;
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
                var redirectUrl = $"http://localhost:5173?accessToken={user.AccessToken}"; // hardcode FE, sau này sẽ thay bằng config
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
    }
}
