using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.OauthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserPoolService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.OauthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserPoolService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Newtonsoft.Json;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.UserAuthService
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IOauthService _oauthService;
        private readonly IUserService _userService;
        private readonly IUserPoolService _userPoolService;
        private readonly ILogger<UserAuthService> _logger;

        public UserAuthService(
            IOauthService oauthService,
            IUserService userService,
            IUserPoolService userPoolService,
            ILogger<UserAuthService> logger)
        {
            _oauthService = oauthService;
            _userService = userService;
            _userPoolService = userPoolService;
            _logger = logger;
        }

        public async Task<FuncResponse<UserAuthenticationTokenResponseModel>> Token(UserAuthenticationTokenRequestModel tokenRequest)
        {
            _logger.LogInformation($"Start UserAuthentication-Token w. data: {tokenRequest.ToJson()}");
            var getUserPoolResult = await _userPoolService.GetUserPool(new UserPoolGetRequestModel
            {
                UserPoolId = tokenRequest.UserPoolId,
            });
            if (getUserPoolResult.IsError())
            {
                _logger.LogInformation(getUserPoolResult.ErrorMessage);
                return new FuncResponse<UserAuthenticationTokenResponseModel>
                {
                    ErrorMessage = "failed to find target user-pool"
                };
            }
            var userPoolData = getUserPoolResult.Data;

            var clientId = userPoolData.ClientId;
            var clientSecret = userPoolData.ClientSecret;
            var redirectUri = userPoolData.RedirectUri;
            var tokenUrl = userPoolData.ExchangeTokenUrl;
            var userInfoUrl = userPoolData.UserInfoUrl;

            var oauthTokenResult = await _oauthService.ExchangeToken(new OauthTokenExchangeRequestModel
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                RedirectUri = redirectUri,
                Code = tokenRequest.Code,
                ExchangeTokenUrl = tokenUrl,
            });
            if (oauthTokenResult.IsError())
            {
                _logger.LogError(oauthTokenResult.ErrorMessage);
                return new FuncResponse<UserAuthenticationTokenResponseModel>
                {
                    ErrorMessage = "failed to exchange token"
                };
            }
            var oauthTokenData = oauthTokenResult.Data;

            var oauthUserInfoResult = await _oauthService.UserInfo(new OauthUserInfoRequestModel
            {
                Token = oauthTokenData.AccessToken,
                UserInfoUrl = userInfoUrl
            });
            if (oauthUserInfoResult.IsError())
            {
                _logger.LogError(oauthUserInfoResult.ErrorMessage);
                return new FuncResponse<UserAuthenticationTokenResponseModel>
                {
                    ErrorMessage = "failed to get userinfo from token"
                };
            }
            var oauthUserInfo = oauthUserInfoResult.Data;
            var oauthUserInfoAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(oauthUserInfo));
            if (oauthUserInfoAsDictionary == null)
            {
                _logger.LogError("failed to parse oauth user info");
                return new FuncResponse<UserAuthenticationTokenResponseModel>
                {
                    ErrorMessage = "failed to parse oauth user info"
                };
            }

            var principalNameKey = userPoolData.PrincipalNameKey;
            if (!oauthUserInfoAsDictionary.ContainsKey(principalNameKey))
            {
                _logger.LogError("oauth user info does not contain principal-name-key");
                return new FuncResponse<UserAuthenticationTokenResponseModel>
                {
                    ErrorMessage = "oauth user info does not contain principal-name-key"
                };
            }

            var principalName = oauthUserInfoAsDictionary.GetValueOrDefault(principalNameKey);
            if (principalName == null)
            {
                _logger.LogError("principal name key does not exist in user info result");
                return new FuncResponse<UserAuthenticationTokenResponseModel>
                {
                    ErrorMessage = "principal name key does not exist in user info result"
                };
            }

            var getUserResult = await _userService.Get(new UserGetRequestModel
            {
                PrincipalName = principalName
            });
            if (getUserResult.IsError())
            {
                _logger.LogInformation("get user failed, will assume user not found");
                var createUserResult = await _userService.Create(new UserCreateRequestModel
                {
                    PrincipalName = principalName,
                    UserPoolId = userPoolData.Id
                });
                if (createUserResult.IsError())
                {
                    _logger.LogError(createUserResult.ErrorMessage);
                    return new FuncResponse<UserAuthenticationTokenResponseModel>
                    {
                        ErrorMessage = "failed to create new user"
                    };
                }
                var createdUser = createUserResult.Data;
                getUserResult.Data = new UserGetResponseModel
                {
                    Id = createdUser.Id,
                    PrincipalName = principalName,
                    UserPoolId = userPoolData.Id
                };
            }
            var userData = getUserResult.Data;

            _logger.LogInformation("set user attributes");
            foreach (var key in oauthUserInfoAsDictionary.Keys)
            {
                if (oauthUserInfoAsDictionary.GetValueOrDefault(key) == null)
                {
                    _logger.LogDebug($"oauthUserInfo key: {key} is null, will skip");
                    continue;
                }

                var updateUserAttributeResult = await _userService.UpdateAttribute(new UserAttributeUpdateRequestModel
                {
                    Key = key,
                    UserId = userData.Id,
                    Value = oauthUserInfoAsDictionary.GetValueOrDefault(key),
                });
                if (updateUserAttributeResult.IsError())
                {
                    _logger.LogError(updateUserAttributeResult.ErrorMessage);
                    _logger.LogInformation("will try to create instead");
                    var createUserAttributeResult = await _userService.CreateAttribute(new UserAttributeCreateRequestModel
                    {
                        Key = key,
                        UserId = userData.Id,
                        Value = oauthUserInfoAsDictionary.GetValueOrDefault(key),
                    });
                    if (createUserAttributeResult.IsError())
                    {
                        _logger.LogError(createUserAttributeResult.ErrorMessage);
                    }
                }
            };

            _logger.LogInformation("success: token request processed");
            return new FuncResponse<UserAuthenticationTokenResponseModel>
            {
                Data = new UserAuthenticationTokenResponseModel
                {
                    ApplicationCallbackUrl = userPoolData.ApplicationCallbackUrl,
                    Token = oauthTokenData.AccessToken,
                    RefreshToken = oauthTokenData.RefreshToken
                }
            };
        }
    }
}
