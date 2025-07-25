using Gender.Services.DuyVK.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gender.APIServices.BE.DuyVK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserAccountsController : ControllerBase
    {
        // =================================
        // === Fields
        // =================================

        private readonly ISystemUserAccountService _userAccountsService;
        public sealed record LoginRequest(string UserName, string Password);

        // =================================
        // === Constructors
        // =================================

        public SystemUserAccountsController(ISystemUserAccountService userAccountsService)
        {
            _userAccountsService = userAccountsService;
        }

        // =================================
        // === Methods
        // =================================

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(
            [FromBody] LoginRequest request)
        {
            // get user account by username and password
            var user = await _userAccountsService.GetUserAccountAsync(request.UserName, request.Password);

            if (user == null)
                return Unauthorized();

            // generate JWT token
            var token = _userAccountsService.GenerateJSONWebToken(user);

            return Ok(new
            {
                token = token
            });
        }
    }
}
