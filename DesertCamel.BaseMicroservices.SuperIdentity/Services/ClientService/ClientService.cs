using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientService
{
    public class ClientService : IClientService
    {
        private readonly SuperIdentityDbContext _superIdentityDbContext;
        private readonly ILogger<ClientService> _logger;

        public ClientService(
            SuperIdentityDbContext superIdentityDbContext,
            ILogger<ClientService> logger)
        {
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
