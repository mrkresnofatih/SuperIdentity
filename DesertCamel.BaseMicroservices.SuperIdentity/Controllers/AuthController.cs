using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserAuthService _userAuthService;

        public AuthController(
            ILogger<AuthController> logger,
            IUserAuthService userAuthService)
        {
            _logger = logger;
            _userAuthService = userAuthService;
        }

        [HttpPost("user-token")]
        public async Task<FuncResponse<UserAuthenticationTokenResponseModel>> Token(UserAuthenticationTokenRequestModel tokenRequest)
        {
            _logger.LogDebug($"start User-Token Endpoint w. data: {tokenRequest.ToJson()}");
            return await _userAuthService.Token(tokenRequest);
        }
    }
}
