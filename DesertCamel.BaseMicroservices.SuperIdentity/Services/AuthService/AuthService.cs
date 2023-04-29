using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.AuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Newtonsoft.Json;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IClientAuthService _clientAuthService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IClientAuthService clientAuthService,
            ILogger<AuthService> logger)
        {
            _clientAuthService = clientAuthService;
            _logger = logger;
        }

        public async Task<FuncResponse<AuthPermitResponseModel>> Permit(AuthPermitRequestModel permitRequest)
        {
            _logger.LogInformation($"Start Permit w. data: {permitRequest.ToJson()}");
            var decodedToken = permitRequest.Token.ToBase64Decode();
            if (decodedToken == null)
            {
                _logger.LogError("failed to decode token");
                return new FuncResponse<AuthPermitResponseModel>
                {
                    ErrorMessage = "failed to decode token"
                };
            }

            var tokenWrapperPayload = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedToken);
            if (tokenWrapperPayload == null)
            {
                _logger.LogError("failed to decode token");
                return new FuncResponse<AuthPermitResponseModel>
                {
                    ErrorMessage = "failed to decode token"
                };
            }

            var tokenType = tokenWrapperPayload.GetValueOrDefault(AppConstants.TokenConstants.TOKEN_TYPE);
            var accessToken = tokenWrapperPayload.GetValueOrDefault(AppConstants.TokenConstants.ACCESS_TOKEN);
            if (tokenType == null || accessToken == null)
            {
                _logger.LogError("invalid token");
                return new FuncResponse<AuthPermitResponseModel>
                {
                    ErrorMessage = "invalid token"
                };
            }

            var calculatedPermitResult = false;
            switch(tokenType)
            {
                case AppConstants.TokenConstants.CLIENT_TOKEN_TYPE:
                    var permitResult = await _clientAuthService.Permit(new ClientAuthPermitRequestModel
                    {
                        AccessToken = accessToken,
                        PermissionName = permitRequest.PermissionName,
                        ResourceName = permitRequest.ResourceName,
                        RoleName = permitRequest.RoleName
                    });
                    if (permitResult.IsError())
                    {
                        _logger.LogError("failed to permit client token");
                        return new FuncResponse<AuthPermitResponseModel>
                        {
                            ErrorMessage = "failed to permit client token"
                        };
                    }
                    calculatedPermitResult = permitResult.Data.IsPermitted;
                    break;
                default:
                    return new FuncResponse<AuthPermitResponseModel>
                    {
                        ErrorMessage = "Unknown client token type"
                    };
            }

            _logger.LogInformation("success: permit client success");
            return new FuncResponse<AuthPermitResponseModel>
            {
                Data = new AuthPermitResponseModel
                {
                    IsPermitted = calculatedPermitResult
                }
            };
        }
    }
}
