using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthorityService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthorityService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientService;
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
        private readonly IClientService _clientService;
        private readonly ILogger<ClientAuthService> _logger;

        public ClientAuthService(
            IOptions<ClientConfig> clientConfig,
            IClientAuthorityService clientAuthorityService,
            IClientService clientService,
            ILogger<ClientAuthService> logger)
        {
            _clientConfig = clientConfig.Value;
            _clientAuthorityService = clientAuthorityService;
            _clientService = clientService;
            _logger = logger;
        }

        public async Task<FuncResponse<ClientAuthTokenResponseModel>> Token(ClientAuthTokenRequestModel tokenRequest)
        {
            _logger.LogInformation($"Start ClientAuthentication-Token w. data: {tokenRequest.ToJson()}");
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
                { "exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds() },
                { "iss", _clientConfig.Issuer },
                { "ath", clientAuthorities },
                { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
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

            _logger.LogInformation("success: ClientAuthentication-Token");
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
