using BiologyRecognition.Application;
using BiologyRecognition.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BiologyRecognition.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _accountService;
        private readonly ILogger<UserAccountController> _logger;

        public UserAccountController(ILogger<UserAccountController> logger, IUserAccountService userAccountService)
        {
            _logger = logger;
            _accountService = userAccountService;
        }

        [HttpGet("GetAllAccount")]
        public async Task<IActionResult> GetAllAccountMemberInfo()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }
    }
}
