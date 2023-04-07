using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Jose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientService
{
    public class ClientService : IClientService
    {
        private readonly ClientConfig _clientConfig;
        private readonly SuperIdentityDbContext _superIdentityDbContext;
        private readonly ILogger<ClientService> _logger;

        public ClientService(
            IOptions<ClientConfig> clientConfig,
            SuperIdentityDbContext superIdentityDbContext,
            ILogger<ClientService> logger)
        {
            _clientConfig = clientConfig.Value;
            _superIdentityDbContext = superIdentityDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<ClientCreateResponseModel>> Create(ClientCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreateClient w. data: {createRequest.ToJson()}");
                var newClient = new ClientEntity
                {
                    Id = Guid.NewGuid(),
                    ClientName = createRequest.ClientName,
                    ClientSecret = _GenerateSecret()
                };

                _superIdentityDbContext.Clients.Add(newClient);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success create client");
                return new FuncResponse<ClientCreateResponseModel>
                {
                    Data = new ClientCreateResponseModel
                    {
                        Id = newClient.Id,
                        ClientName = newClient.ClientName,
                        ClientSecret = newClient.ClientSecret
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to create client");
                return new FuncResponse<ClientCreateResponseModel>
                {
                    ErrorMessage = "failed to create client"
                };
            }
        }

        public async Task<FuncResponse<ClientDeleteResponseModel>> Delete(ClientDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start ClientDelete w. data: {deleteRequest.ToJson()}");
                var foundClient = await _superIdentityDbContext
                    .Clients
                    .Where(x => x.ClientName.Equals(deleteRequest.ClientName))
                    .FirstOrDefaultAsync();
                if (foundClient == null)
                {
                    throw new Exception("client for delete op is not found");
                }

                _superIdentityDbContext.Clients.Remove(foundClient);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success deleteClient");
                return new FuncResponse<ClientDeleteResponseModel>
                {
                    Data = new ClientDeleteResponseModel
                    {
                        ClientName = deleteRequest.ClientName
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to delete client");
                return new FuncResponse<ClientDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete client"
                };
            }
        }

        public async Task<FuncResponse<ClientGetResponseModel>> Get(ClientGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start GetClient w. data: {getRequest.ToJson()}");
                var foundClient = await _superIdentityDbContext
                    .Clients
                    .Where(x => x.ClientName.Equals(getRequest.ClientName))
                    .FirstOrDefaultAsync();
                if (foundClient == null)
                {
                    throw new Exception("client for get op is not found");
                }

                _logger.LogInformation("success get client");
                return new FuncResponse<ClientGetResponseModel>
                {
                    Data = new ClientGetResponseModel
                    {
                        Id = foundClient.Id,
                        ClientName = foundClient.ClientName,
                        ClientSecret = foundClient.ClientSecret,
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to get client");
                return new FuncResponse<ClientGetResponseModel>
                {
                    ErrorMessage = "failed to get client"
                };
            }
        }

        public async Task<FuncListResponse<ClientGetResponseModel>> List(ClientListRequestModel listRequest)
        {
            try
            {
                _logger.LogInformation($"Start listClients w. data: {listRequest.ToJson()}");
                var query = _superIdentityDbContext.Clients.AsQueryable();

                if (!String.IsNullOrWhiteSpace(listRequest.Name))
                {
                    query = query.Where(x => x.ClientName.Contains(listRequest.Name));
                }

                var total = await query.CountAsync();

                query = query.OrderBy(x => x.ClientName);

                var foundClients = await query
                    .Skip((int)listRequest.PageSize * ((int)listRequest.Page - 1))
                    .Take((int)listRequest.PageSize)
                    .Select(x => new ClientGetResponseModel
                    {
                        Id = x.Id,
                        ClientName = x.ClientName,
                        ClientSecret = x.ClientSecret,
                    })
                    .ToListAsync();

                _logger.LogInformation("success list clients");
                return new FuncListResponse<ClientGetResponseModel>
                {
                    Total = total,
                    Data = foundClients,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to list clients");
                return new FuncListResponse<ClientGetResponseModel>
                {
                    ErrorMessage = "failed to list clients"
                };
            }
        }

        public async Task<FuncResponse<ClientRotateResponseModel>> Rotate(ClientRotateRequestModel rotateRequest)
        {
            try
            {
                _logger.LogInformation($"Start RotateClientSecret w. data: {rotateRequest.ToJson()}");
                var foundClient = _superIdentityDbContext
                    .Clients
                    .Where(x => x.ClientName.Equals(rotateRequest.ClientName))
                    .FirstOrDefault();
                if (foundClient == null)
                {
                    throw new Exception("client for rotate secrets op not found");
                }

                foundClient.ClientSecret = _GenerateSecret();
                _superIdentityDbContext.Clients.Update(foundClient);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success update permission");
                return new FuncResponse<ClientRotateResponseModel>
                {
                    Data = new ClientRotateResponseModel()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to rotate client secrets");
                return new FuncResponse<ClientRotateResponseModel>
                {
                    ErrorMessage = "failed to rotate client secrets"
                };
            }
        }

        public async Task<FuncResponse<ClientTokenResponseModel>> Token(ClientTokenRequestModel tokenRequest)
        {
            try
            {
                _logger.LogInformation($"Start Token w. data: {tokenRequest.ToJson()}");
                var foundClient = await _superIdentityDbContext
                    .Clients
                    .Where(x => x.ClientName.Equals(tokenRequest.ClientName))
                    .FirstOrDefaultAsync();
                if (foundClient == null)
                {
                    throw new Exception("Unknown credentials");
                }
                if (!foundClient.ClientSecret.Equals(tokenRequest.ClientSecret))
                {
                    throw new Exception("Invalid credentials");
                }

                _logger.LogInformation("success validating credentials");
                var generateTokenResult = _GenerateToken(new ClientGenerateTokenRequest
                {
                    ClientName = foundClient.ClientName,
                });
                if (generateTokenResult.IsError())
                {
                    throw new Exception("failed to generate client token");
                }

                _logger.LogInformation("success generate token");
                return new FuncResponse<ClientTokenResponseModel>
                {
                    Data = new ClientTokenResponseModel
                    {
                        Token = generateTokenResult.Data
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to get token");
                return new FuncResponse<ClientTokenResponseModel>
                {
                    ErrorMessage = e.Message
                };
            }
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
            catch(Exception e)
            {
                _logger.LogError(e, "failed to generate token");
                return new FuncResponse<string>
                {
                    ErrorMessage = "failed ot generate token"
                };
            }
        }

        class ClientGenerateTokenRequest
        {
            public string ClientName { get; set; }
        }

        private string _GenerateSecret()
        {
            Random res = new Random();

            // String that contain both alphabets and numbers
            String str = "abcdefghijklmnopqrstuvwxyz~!@#$%*?ABCDEFGHIJKLMNOPQRSTUVXYZ0123456789";
            int size = 24;

            // Initializing the empty string
            String randomstring = "";

            for (int i = 0; i < size; i++)
            {

                // Selecting a index randomly
                int x = res.Next(str.Length);

                // Appending the character at the 
                // index to the random alphanumeric string.
                randomstring = randomstring + str[x];
            }

            return randomstring;
        }
    }
}
