using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientRoleService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RolePermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleResourceService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientRoleService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RolePermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RoleResourceService;
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
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IRoleResourceService _roleResourceService;
        private readonly IClientRoleService _clientRoleService;
        private readonly IClientService _clientService;
        private readonly ILogger<ClientAuthService> _logger;

        public ClientAuthService(
            IOptions<ClientConfig> clientConfig,
            IRolePermissionService rolePermissionService,
            IRoleResourceService roleResourceService,
            IClientRoleService clientRoleService,
            IClientService clientService,
            ILogger<ClientAuthService> logger)
        {
            _clientConfig = clientConfig.Value;
            _rolePermissionService = rolePermissionService;
            _roleResourceService = roleResourceService;
            _clientRoleService = clientRoleService;
            _clientService = clientService;
            _logger = logger;
        }

        public async Task<FuncResponse<ClientAuthAssignResponseModel>> Assign(ClientAuthAssignRequestModel assignRequest)
        {
            _logger.LogInformation($"Start ClientAuth-Assign w. data: {assignRequest.ToJson()}");
            var clientGetResult = await _clientService.Get(new ClientGetRequestModel
            {
                ClientName = assignRequest.ClientName,
            });
            if (clientGetResult.IsError())
            {
                _logger.LogError(clientGetResult.ErrorMessage);
                return new FuncResponse<ClientAuthAssignResponseModel>
                {
                    ErrorMessage = "get client error"
                };
            }

            var roleResourceGetResult = await _roleResourceService.Get(new RoleResourceGetRequestModel
            {
                RoleName = assignRequest.RoleName,
                ResourceName = assignRequest.ResourceName
            });
            if (roleResourceGetResult.IsError())
            {
                _logger.LogError("role-resource for assign ops not found");
                return new FuncResponse<ClientAuthAssignResponseModel>
                {
                    ErrorMessage = "role-resource for assign op not found"
                };
            }

            var clientRoleCreateResult = await _clientRoleService
                .Create(new ClientRoleCreateRequestModel
                {
                    ClientName = clientGetResult.Data.ClientName,
                    ResourceName = roleResourceGetResult.Data.ResourceName,
                    RoleName = roleResourceGetResult.Data.RoleName,
                });
            if (clientRoleCreateResult.IsError())
            {
                _logger.LogError("client role create failed");
                return new FuncResponse<ClientAuthAssignResponseModel>
                {
                    ErrorMessage = "client role create failed"
                };
            }

            _logger.LogInformation("success: create client role");
            return new FuncResponse<ClientAuthAssignResponseModel>
            {
                Data = new ClientAuthAssignResponseModel()
            };
        }

        public async Task<FuncResponse<ClientAuthLoginResponseModel>> Login(ClientAuthLoginRequestModel loginRequest)
        {
            _logger.LogInformation($"Start ClientAuth-Login w. data: {loginRequest.ToJson()}");
            var getClientResult = await _clientService.Get(new ClientGetRequestModel
            {
                ClientName = loginRequest.ClientName
            });
            if (getClientResult.IsError())
            {
                _logger.LogError(getClientResult.ErrorMessage);
                return new FuncResponse<ClientAuthLoginResponseModel>
                {
                    ErrorMessage = "unknown credentials"
                };
            }

            var isClientCredentialsValid = loginRequest.ClientSecret.Equals(getClientResult.Data.ClientSecret);
            if (isClientCredentialsValid)
            {
                _logger.LogError("invalid client credentials");
                return new FuncResponse<ClientAuthLoginResponseModel>
                {
                    ErrorMessage = "invalid credentials"
                };
            }

            var generateTokenResult = _GenerateToken(new ClientGenerateTokenRequest
            {
                ClientName = loginRequest.ClientName,
            });
            if (generateTokenResult.IsError())
            {
                _logger.LogError(generateTokenResult.ErrorMessage);
                return new FuncResponse<ClientAuthLoginResponseModel>
                {
                    ErrorMessage = "failed to generate token"
                };
            }
            var accessToken = generateTokenResult.Data;
            _logger.LogInformation("success: clientauth-login");
            return new FuncResponse<ClientAuthLoginResponseModel>
            {
                Data = new ClientAuthLoginResponseModel
                {
                    AccessToken = accessToken,
                }
            };
        }

        private FuncResponse<string> _GenerateToken(ClientGenerateTokenRequest generateTokenRequest)
        {
            try
            {
                _logger.LogInformation($"Start _GenerateToken w. data: {generateTokenRequest.ToJson()}");
                var payload = new Dictionary<string, object>
                {
                    { "sub", generateTokenRequest.ClientName },
                    { "exp", DateTimeOffset.UtcNow.AddMinutes(60).ToUnixTimeSeconds() },
                    { "iss", _clientConfig.Issuer },
                };
                var byteSecret = Encoding.ASCII.GetBytes(_clientConfig.Secret);
                var token = Jose.JWT.Encode(payload, byteSecret, JwsAlgorithm.HS256);

                _logger.LogInformation("success generate token");
                return new FuncResponse<string>
                {
                    Data = token
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to generate token");
                return new FuncResponse<string>
                {
                    ErrorMessage = "failed ot generate token"
                };
            }
        }

        private FuncResponse<Dictionary<string, string>> _ExtractToken(ClientExtractTokenRequest extractTokenRequest)
        {
            try
            {
                _logger.LogInformation($"Start _ExtractToken w. data: {extractTokenRequest.ToJson()}");
                var byteSecret = Encoding.ASCII.GetBytes(_clientConfig.Secret);
                var json = Jose.JWT.Decode(extractTokenRequest.AccessToken, byteSecret, JwsAlgorithm.HS256);
                var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                _logger.LogInformation("success: _ExtractToken");
                return new FuncResponse<Dictionary<string, string>>
                {
                    Data = claims
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to extract token");
                return new FuncResponse<Dictionary<string, string>>
                {
                    ErrorMessage = "failed to extract token"
                };
            }
        }

        class ClientGenerateTokenRequest
        {
            public string ClientName { get; set; }
        }

        class ClientExtractTokenRequest
        {
            public string AccessToken { get; set; }
        }

        public async Task<FuncResponse<ClientAuthPermitResponseModel>> Permit(ClientAuthPermitRequestModel permitRequest)
        {
            _logger.LogInformation($"Start ClientAuth-Permit w. data: {permitRequest.ToJson()}");
            var extractTokenResult = _ExtractToken(new ClientExtractTokenRequest
            {
                AccessToken = permitRequest.AccessToken
            });
            if (extractTokenResult.IsError())
            {
                _logger.LogError(extractTokenResult.ErrorMessage);
                return new FuncResponse<ClientAuthPermitResponseModel>
                {
                    ErrorMessage = "invalid token"
                };
            }
            var claims = extractTokenResult.Data;
            var subClaim = claims.GetValueOrDefault("sub");
            if (subClaim == null)
            {
                _logger.LogInformation("sub claim is null, will fail");
                return new FuncResponse<ClientAuthPermitResponseModel>
                {
                    ErrorMessage = "invalid token claims"
                };
            }

            var clientRoleGetResult = await _clientRoleService.Get(new ClientRoleGetRequestModel
            {
                ClientName = subClaim,
                ResourceName = permitRequest.ResourceName,
                RoleName = permitRequest.RoleName
            });
            if (clientRoleGetResult.IsError())
            {
                _logger.LogError("client role is not found");
                return new FuncResponse<ClientAuthPermitResponseModel>
                {
                    ErrorMessage = "client role not found"
                };
            }
            var clientRoleData = clientRoleGetResult.Data;

            var rolePermissionGetResult = await _rolePermissionService.Get(new RolePermissionGetRequestModel
            {
                PermissionName = permitRequest.PermissionName,
                RoleName = clientRoleData.RoleName
            });
            if (rolePermissionGetResult.IsError())
            {
                _logger.LogError("role permission get is not found");
                return new FuncResponse<ClientAuthPermitResponseModel>
                {
                    ErrorMessage = "role permission not found"
                };
            }

            _logger.LogInformation("role permission exists! client is permitted");
            return new FuncResponse<ClientAuthPermitResponseModel>
            {
                Data = new ClientAuthPermitResponseModel
                {
                    IsPermitted = true
                }
            };
        }
    }
}
