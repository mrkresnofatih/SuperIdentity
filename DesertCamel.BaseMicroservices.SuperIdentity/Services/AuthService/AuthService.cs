using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.AuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Newtonsoft.Json;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IClientAuthService _clientAuthService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserAuthService userAuthService,
            IClientAuthService clientAuthService,
            ILogger<AuthService> logger)
        {
            _userAuthService = userAuthService;
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
                    var clientPermitResult = await _clientAuthService.Permit(new ClientAuthPermitRequestModel
                    {
                        AccessToken = accessToken,
                        PermissionName = permitRequest.PermissionName,
                        ResourceName = permitRequest.ResourceName,
                        RoleName = permitRequest.RoleName
                    });
                    if (clientPermitResult.IsError())
                    {
                        _logger.LogError("failed to permit client token");
                        return new FuncResponse<AuthPermitResponseModel>
                        {
                            ErrorMessage = "failed to permit client token"
                        };
                    }
                    calculatedPermitResult = clientPermitResult.Data.IsPermitted;
                    break;
                case AppConstants.TokenConstants.USER_TOKEN_TYPE:
                    var userPoolId = tokenWrapperPayload.GetValueOrDefault(AppConstants.TokenConstants.USER_POOL_ID);
                    if (userPoolId == null)
                    {
                        _logger.LogError("user-pool-id not found");
                        return new FuncResponse<AuthPermitResponseModel>
                        {
                            ErrorMessage = "failed to find userpoolid in token"
                        };
                    }
                    var userPermitResult = await _userAuthService.Permit(new UserAuthPermitRequestModel
                    {
                        AccessToken = accessToken,
                        UserPoolId = Guid.Parse(userPoolId),
                        PermissionName = permitRequest.PermissionName,
                        ResourceName = permitRequest.ResourceName,
                        RoleName = permitRequest.RoleName,
                    });
                    if (userPermitResult.IsError())
                    {
                        _logger.LogError("failed to permit user token");
                        return new FuncResponse<AuthPermitResponseModel>
                        {
                            ErrorMessage = "failed to permit user token"
                        };
                    }
                    calculatedPermitResult = userPermitResult.Data.IsPermitted;
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
