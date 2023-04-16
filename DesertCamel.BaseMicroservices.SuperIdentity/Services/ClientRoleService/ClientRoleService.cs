using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientRoleService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientRoleService
{
    public class ClientRoleService : IClientRoleService
    {
        private readonly SuperIdentityDbContext _superIdentityDbContext;
        private readonly ILogger<ClientRoleService> _logger;

        public ClientRoleService(
            SuperIdentityDbContext superIdentityDbContext,
            ILogger<ClientRoleService> logger)
        {
            _superIdentityDbContext = superIdentityDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<ClientRoleCreateResponseModel>> Create(ClientRoleCreateRequestModel model)
        {
            try
            {
                _logger.LogInformation($"Start Create ClientRole w. data: {model.ToJson()}");
                var newClientRole = new ClientRoleEntity
                {
                    Id = Guid.NewGuid(),
                    ClientName = model.ClientName,
                    ResourceName = model.ResourceName,
                    RoleName = model.RoleName
                };
                _superIdentityDbContext.ClientRoles.Add(newClientRole);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success: create client-role");
                return new FuncResponse<ClientRoleCreateResponseModel>
                {
                    Data = new ClientRoleCreateResponseModel
                    {
                        Id = newClientRole.Id,
                        ClientName = newClientRole.ClientName,
                        ResourceName = newClientRole.ResourceName,
                        RoleName = newClientRole.RoleName
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to create client-role");
                return new FuncResponse<ClientRoleCreateResponseModel>
                {
                    ErrorMessage = "failed to create client-role"
                };
            }
        }

        public async Task<FuncResponse<ClientRoleDeleteResponseModel>> Delete(ClientRoleDeleteRequestModel model)
        {
            try
            {
                _logger.LogInformation($"Start Delete ClientRole w. data: {model.ToJson()}");
                var foundClientRole = await _superIdentityDbContext.ClientRoles
                    .Where(x => x.ClientName.Equals(model.ClientName) && x.ResourceName.Equals(model.ResourceName) && x.RoleName.Equals(model.RoleName))
                    .FirstOrDefaultAsync();
                if (foundClientRole == null)
                {
                    throw new Exception("ClientRole for delete op not found");
                }

                _superIdentityDbContext.ClientRoles.Remove(foundClientRole);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("Success: delete client-role");
                return new FuncResponse<ClientRoleDeleteResponseModel>
                {
                    Data = new ClientRoleDeleteResponseModel()
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to delete client role");
                return new FuncResponse<ClientRoleDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete client role"
                };
            }
        }

        public async Task<FuncResponse<ClientRoleGetResponseModel>> Get(ClientRoleGetRequestModel model)
        {
            try
            {
                _logger.LogInformation($"Start Get ClientRole w. data: {model.ToJson()}");
                var foundClientRole = await _superIdentityDbContext.ClientRoles
                    .Where(x => x.ClientName.Equals(model.ClientName) && x.ResourceName.Equals(model.ResourceName) && x.RoleName.Equals(model.RoleName))
                    .FirstOrDefaultAsync();
                if (foundClientRole == null)
                {
                    throw new Exception("ClientRole for get op not found");
                }

                _logger.LogInformation("Success: delete client-role");
                return new FuncResponse<ClientRoleGetResponseModel>
                {
                    Data = new ClientRoleGetResponseModel
                    {
                        Id = foundClientRole.Id,
                        ClientName = foundClientRole.ClientName,
                        RoleName = foundClientRole.RoleName,
                        ResourceName = foundClientRole.ResourceName
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to get client role");
                return new FuncResponse<ClientRoleGetResponseModel>
                {
                    ErrorMessage = "failed to get client role"
                };
            }
        }

        public async Task<FuncListResponse<ClientRoleGetResponseModel>> List(ClientRoleListRequestModel model)
        {
            try
            {
                _logger.LogInformation($"Start List ClientRoles w. data: {model.ToJson()}");
                var query = _superIdentityDbContext.ClientRoles.AsQueryable();

                if (!String.IsNullOrWhiteSpace(model.RoleName))
                {
                    query = query.Where(x => x.RoleName.Contains(model.RoleName));
                }

                if (!String.IsNullOrWhiteSpace(model.ResourceName))
                {
                    query = query.Where(x => x.ResourceName.Contains(model.ResourceName));
                }

                var total = await query.CountAsync();

                query = query.OrderBy(x => x.RoleName);

                var foundClientRoles = await query
                    .Skip((model.Page - 1) * model.PageSize)
                    .Take(model.PageSize)
                    .Select(x => new ClientRoleGetResponseModel
                    {
                        Id = x.Id,
                        ClientName = x.ClientName,
                        ResourceName = x.ResourceName,
                        RoleName = x.RoleName,
                    })
                    .ToListAsync();

                _logger.LogInformation("success: list clientroles");
                return new FuncListResponse<ClientRoleGetResponseModel>
                {
                    Data = foundClientRoles,
                    Total = total,
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to list clientroles");
                return new FuncListResponse<ClientRoleGetResponseModel>
                {
                    ErrorMessage = "failed to list client roles"
                };
            }
        }
    }
}
