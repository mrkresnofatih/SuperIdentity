using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthService;
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
        private readonly IClientAuthService _clientAuthService;
        private readonly IUserAuthService _userAuthService;

        public AuthController(
            ILogger<AuthController> logger,
            IClientAuthService clientAuthService,
            IUserAuthService userAuthService)
        {
            _logger = logger;
            _clientAuthService = clientAuthService;
            _userAuthService = userAuthService;
        }

        [HttpPost("client-token")]
        public async Task<FuncResponse<ClientAuthTokenResponseModel>> ClientToken(ClientAuthTokenRequestModel tokenRequest)
        {
            _logger.LogDebug($"start Client-Token Endpoint w. data: {tokenRequest.ToJson()}");
            return await _clientAuthService.Token(tokenRequest);
        }

        [HttpPost("user-token")]
        public async Task<FuncResponse<UserAuthenticationTokenResponseModel>> UserToken(UserAuthenticationTokenRequestModel tokenRequest)
        {
            _logger.LogDebug($"start User-Token Endpoint w. data: {tokenRequest.ToJson()}");
            return await _userAuthService.Token(tokenRequest);
        }

    }
}
