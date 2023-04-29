using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthorityService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RolePermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthorityService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RolePermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Jose;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthService
{
    public class ClientAuthService : IClientAuthService
    {
        private readonly ClientConfig _clientConfig;
        private readonly IClientAuthorityService _clientAuthorityService;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IClientService _clientService;
        private readonly ILogger<ClientAuthService> _logger;

        public ClientAuthService(
            IOptions<ClientConfig> clientConfig,
            IClientAuthorityService clientAuthorityService,
            IRolePermissionService rolePermissionService,
            IClientService clientService,
            ILogger<ClientAuthService> logger)
        {
            _clientConfig = clientConfig.Value;
            _clientAuthorityService = clientAuthorityService;
            _rolePermissionService = rolePermissionService;
            _clientService = clientService;
            _logger = logger;
        }

        public async Task<FuncResponse<ClientAuthPermitResponseModel>> Permit(ClientAuthPermitRequestModel permitRequest)
        {
            _logger.LogInformation($"Start Permit w. data: {permitRequest.ToJson()}");
            try
            {
                var secret = Encoding.ASCII.GetBytes(_clientConfig.Code);
                var json = Jose.JWT.Decode(permitRequest.AccessToken, secret, JwsAlgorithm.HS256);
                if (json == null)
                {
                    throw new Exception("decoded access token is null");
                }
                var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                if (claims == null)
                {
                    throw new Exception("cannot parse token claims");
                }
                var issuer = claims.GetValueOrDefault("iss");
                if (issuer == null)
                {
                    throw new Exception("invalid access token");
                }
                var isIssuerValid = issuer.Equals(_clientConfig.Issuer);
                if (!isIssuerValid)
                {
                    throw new Exception("invalid access token");
                }
                var clientName = claims.GetValueOrDefault("sub");
                if (String.IsNullOrWhiteSpace(clientName))
                {
                    throw new Exception("invalid access token");
                }

                var clientAuthorityGetResult = await _clientAuthorityService.Get(new ClientAuthorityGetRequestModel
                {
                    ClientName = clientName,
                    RoleResourceId = $"{permitRequest.RoleName}-{permitRequest.ResourceName}"
                });
                if (clientAuthorityGetResult.IsError())
                {
                    _logger.LogError("client does not have requested client authority");
                    throw new Exception(clientAuthorityGetResult.ErrorMessage);
                }

                var rolePermissionGetResult = await _rolePermissionService.Get(new RolePermissionGetRequestModel
                {
                    PermissionName = permitRequest.PermissionName,
                    RoleName = permitRequest.RoleName
                });
                if (rolePermissionGetResult.IsError())
                {
                    _logger.LogError("role does not have requested permission name");
                    throw new Exception("role does not have requested permission name");
                }

                _logger.LogInformation("success: permit client");
                return new FuncResponse<ClientAuthPermitResponseModel>
                {
                    Data = new ClientAuthPermitResponseModel
                    {
                        IsPermitted = true,
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to process permit-client");
                return new FuncResponse<ClientAuthPermitResponseModel>
                {
                    ErrorMessage = "Failed to process permit-client"
                };
            }
        }

        public async Task<FuncResponse<ClientAuthTokenResponseModel>> Token(ClientAuthTokenRequestModel tokenRequest)
        {
            _logger.LogInformation($"Start ClientAuthentication-AccessToken w. data: {tokenRequest.ToJson()}");
            var getClientResult = await _clientService.Get(new ClientGetRequestModel
            {
                ClientName = tokenRequest.ClientName
            });
            if (getClientResult.IsError())
            {
                _logger.LogError(getClientResult.ErrorMessage);
                return new FuncResponse<ClientAuthTokenResponseModel>
                {
                    ErrorMessage = "invalid client"
                };
            }
            var clientDetails = getClientResult.Data;

            var isClientCredentialsValid = _GetClientCredentialsValidity(clientDetails.ClientSecret, tokenRequest.ClientSecret);
            if (!isClientCredentialsValid)
            {
                _logger.LogError("invalid client credentials");
                return new FuncResponse<ClientAuthTokenResponseModel>
                {
                    ErrorMessage = "invalid client credentials"
                };
            }

            var listClientAuthoritiesResult = await _clientAuthorityService.List(new ClientAuthorityListRequestModel
            {
                ClientName = tokenRequest.ClientName,
                Page = 1,
                PageSize = 50
            });
            if (listClientAuthoritiesResult.IsError())
            {
                _logger.LogError("failed to list client authorities");
                return new FuncResponse<ClientAuthTokenResponseModel>
                {
                    ErrorMessage = "failed to list client authorities"
                };
            }
            var clientAuthorities = listClientAuthoritiesResult.Data;

            var payload = new Dictionary<string, object>
            {
                { "sub", clientDetails.ClientName },
                { "lft", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds().ToString() },
                { "iss", _clientConfig.Issuer },
                { "ath", clientAuthorities.ToJson() },
                { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
            };
            var secret = Encoding.ASCII.GetBytes(_clientConfig.Code);
            var jwt = Jose.JWT.Encode(payload, secret, JwsAlgorithm.HS256);
            if (jwt == null)
            {
                _logger.LogError("token generated is null, will fail");
                return new FuncResponse<ClientAuthTokenResponseModel>
                {
                    ErrorMessage = "failed to generate token"
                };
            }

            var tokenWrapperPayload = new Dictionary<string, string>
            {
                { AppConstants.TokenConstants.TOKEN_TYPE, AppConstants.TokenConstants.CLIENT_TOKEN_TYPE },
                { AppConstants.TokenConstants.ACCESS_TOKEN, jwt }
            };
            var jsonTokenWrapperPayload = JsonConvert.SerializeObject(tokenWrapperPayload);
            if (jsonTokenWrapperPayload == null)
            {
                _logger.LogError("failed to generate token");
                return new FuncResponse<ClientAuthTokenResponseModel>
                {
                    ErrorMessage = "failed to generate token"
                };
            }
            var token = jsonTokenWrapperPayload.ToBase64Encode();
            if (token == null)
            {
                _logger.LogError("failed to generate token");
                return new FuncResponse<ClientAuthTokenResponseModel>
                {
                    ErrorMessage = "failed to generate token"
                };
            }

            _logger.LogInformation("success: ClientAuthentication-AccessToken");
            return new FuncResponse<ClientAuthTokenResponseModel>
            {
                Data = new ClientAuthTokenResponseModel
                {
                    LifeTime = 3600,
                    Token = token
                }
            };
        }

        private bool _GetClientCredentialsValidity(string secret, string challenge)
        {
            return secret.Equals(challenge);
        }
    }
}
