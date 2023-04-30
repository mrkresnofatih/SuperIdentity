using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.AuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.AuthService;
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
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IClientAuthService _clientAuthService;
        private readonly IUserAuthService _userAuthService;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger,
            IClientAuthService clientAuthService,
            IUserAuthService userAuthService)
        {
            _authService = authService;
            _logger = logger;
            _clientAuthService = clientAuthService;
            _userAuthService = userAuthService;
        }

        [HttpPost("client-token")]
        public async Task<FuncResponse<ClientAuthTokenResponseModel>> ClientToken(ClientAuthTokenRequestModel tokenRequest)
        {
            _logger.LogDebug($"start Client-AccessToken Endpoint w. data: {tokenRequest.ToJson()}");
            return await _clientAuthService.Token(tokenRequest);
        }

        [HttpPost("user-token")]
        public async Task<FuncResponse<UserAuthTokenResponseModel>> UserToken(UserAuthTokenRequestModel tokenRequest)
        {
            _logger.LogDebug($"start User-AccessToken Endpoint w. data: {tokenRequest.ToJson()}");
            return await _userAuthService.Token(tokenRequest);
        }

        [HttpPost("permit")]
        public async Task<FuncResponse<AuthPermitResponseModel>> Permit(AuthPermitRequestModel permitRequest)
        {
            return await _authService.Permit(permitRequest);
        }
    }
}
