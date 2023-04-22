using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthorityService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthorityService
{
    public class ClientAuthorityService : IClientAuthorityService
    {
        private readonly SuperIdentityDbContext _superIdentityDbContext;
        private readonly ILogger<ClientAuthorityService> _logger;

        public ClientAuthorityService(
            SuperIdentityDbContext superIdentityDbContext,
            ILogger<ClientAuthorityService> logger)
        {
            _superIdentityDbContext = superIdentityDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<ClientAuthorityAddResponseModel>> Add(ClientAuthorityAddRequestModel createRequest)
        {
            _logger.LogInformation($"Start AddClientAuthority w. data: {createRequest.ToJson()}");
            try
            {
                var foundClientAuthority = await _superIdentityDbContext
                    .ClientAuthorities
                    .Where(x => x.ClientName.Equals(createRequest.ClientName) && x.RoleResourceId.Equals(createRequest.RoleResourceId))
                    .FirstOrDefaultAsync();
                if (foundClientAuthority != null)
                {
                    _logger.LogInformation("client authority w. client-name & role-resource-id already exists");
                    return new FuncResponse<ClientAuthorityAddResponseModel>
                    {
                        Data = new ClientAuthorityAddResponseModel
                        {
                            Id = foundClientAuthority.Id,
                            ClientName = foundClientAuthority.ClientName,
                            RoleResourceId = foundClientAuthority.RoleResourceId
                        }
                    };
                }

                var newClientAuthority = new ClientAuthorityEntity
                {
                    Id = Guid.NewGuid(),
                    ClientName = createRequest.ClientName,
                    RoleResourceId = createRequest.RoleResourceId
                };
                _superIdentityDbContext.ClientAuthorities.Add(newClientAuthority);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("client-authority add success");
                return new FuncResponse<ClientAuthorityAddResponseModel>
                {
                    Data = new ClientAuthorityAddResponseModel
                    {
                        Id = newClientAuthority.Id,
                        ClientName = newClientAuthority.ClientName,
                        RoleResourceId = newClientAuthority.RoleResourceId
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to add client authority");
                return new FuncResponse<ClientAuthorityAddResponseModel>
                {
                    ErrorMessage = "failed to add client authority"
                };
            }
        }

        public async Task<FuncResponse<ClientAuthorityDeleteResponseModel>> Delete(ClientAuthorityDeleteRequestModel deleteRequest)
        {
            _logger.LogInformation($"Start DeleteClientAuthority w. data: {deleteRequest.ToJson()}");
            try
            {
                var foundClientAuthority = await _superIdentityDbContext
                    .ClientAuthorities
                    .Where(x => x.ClientName.Equals(deleteRequest.ClientName) && x.RoleResourceId.Equals(deleteRequest.RoleResourceId))
                    .FirstOrDefaultAsync();
                if (foundClientAuthority == null)
                {
                    _logger.LogInformation("client authority w. client-name & role-resource-id already deleted");
                    return new FuncResponse<ClientAuthorityDeleteResponseModel>
                    {
                        Data = new ClientAuthorityDeleteResponseModel()
                    };
                }

                _superIdentityDbContext.ClientAuthorities.Remove(foundClientAuthority);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("client-authority delete success");
                return new FuncResponse<ClientAuthorityDeleteResponseModel>
                {
                    Data = new ClientAuthorityDeleteResponseModel()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to delete client authority");
                return new FuncResponse<ClientAuthorityDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete client authority"
                };
            }
        }

        public async Task<FuncResponse<ClientAuthorityGetResponseModel>> Get(ClientAuthorityGetRequestModel getRequest)
        {
            _logger.LogInformation($"Start GetClientAuthority w. data: {getRequest.ToJson()}");
            try
            {
                var foundClientAuthority = await _superIdentityDbContext
                    .ClientAuthorities
                    .Where(x => x.ClientName.Equals(getRequest.ClientName) && x.RoleResourceId.Equals(getRequest.RoleResourceId))
                    .FirstOrDefaultAsync();
                if (foundClientAuthority != null)
                {
                    _logger.LogInformation("get client authority w. client-name & role-resource-id success");
                    return new FuncResponse<ClientAuthorityGetResponseModel>
                    {
                        Data = new ClientAuthorityGetResponseModel
                        {
                            ClientName = foundClientAuthority.ClientName,
                            RoleResourceId = foundClientAuthority.RoleResourceId,
                            Id = foundClientAuthority.Id
                        }
                    };
                }

                throw new Exception("Client-Authority not found");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to get client authority");
                return new FuncResponse<ClientAuthorityGetResponseModel>
                {
                    ErrorMessage = "failed to get client authority"
                };
            }
        }

        public async Task<FuncListResponse<ClientAuthorityGetResponseModel>> List(ClientAuthorityListRequestModel listRequest)
        {
            _logger.LogInformation($"Start ListClientAuthority w. data: {listRequest.ToJson()}");
            try
            {
                var query = _superIdentityDbContext
                    .ClientAuthorities
                    .Where(x => x.ClientName.Equals(listRequest.ClientName));
                var total = await query.CountAsync();
                var foundClientAuthorities = await query
                    .OrderBy(x => x.RoleResourceId)
                    .Skip(listRequest.PageSize * (listRequest.Page - 1))
                    .Take(listRequest.PageSize)
                    .Select(x => new ClientAuthorityGetResponseModel
                    {
                        Id = x.Id,
                        ClientName = x.ClientName,
                        RoleResourceId = x.RoleResourceId
                    })
                    .ToListAsync();

                _logger.LogInformation("success: listClientAuthority");
                return new FuncListResponse<ClientAuthorityGetResponseModel>
                {
                    Data = foundClientAuthorities,
                    Total = total,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to list client authority");
                return new FuncListResponse<ClientAuthorityGetResponseModel>
                {
                    ErrorMessage = "failed to list client authority"
                };
            }
        }
    }
}
