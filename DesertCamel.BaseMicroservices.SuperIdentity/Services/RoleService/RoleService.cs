using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly SuperCognitoDbContext _superCognitoDbContext;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            SuperCognitoDbContext superCognitoDbContext,
            ILogger<RoleService> logger)
        {
            this._superCognitoDbContext = superCognitoDbContext;
            this._logger = logger;
        }

        public async Task<FuncResponse<RoleCreateResponseModel>> CreateRole(RoleCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreateRole w. data: {createRequest.ToJson()}");
                var newRole = new RoleEntity
                {
                    Id = Guid.NewGuid(),
                    Name = createRequest.Name,
                    Description = createRequest.Description,
                };

                await _superCognitoDbContext.Roles.AddAsync(newRole);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("Finish: CreateRole success");
                return new FuncResponse<RoleCreateResponseModel>
                {
                    Data = new RoleCreateResponseModel
                    {
                        Id = newRole.Id,
                        Name = newRole.Name,
                        Description = newRole.Description,
                    },
                    ErrorMessage = null
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "CreateRoleAPI failed");
                return new FuncResponse<RoleCreateResponseModel>
                {
                    ErrorMessage = "CreateRoleAPI failed"
                };
            }
        }

        public async Task<FuncResponse<RoleDeleteResponseModel>> DeleteRole(RoleDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start DeleteRole w. data: {deleteRequest.ToJson()}");
                var foundRole = await _superCognitoDbContext
                    .Roles
                    .Where(x => x.Name == deleteRequest.Name)
                    .FirstOrDefaultAsync();
                if (foundRole == null)
                {
                    throw new Exception("Role for delete not found");
                }

                _superCognitoDbContext.Roles.Remove(foundRole);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("DeleteRole success");
                return new FuncResponse<RoleDeleteResponseModel>
                {
                    Data = new RoleDeleteResponseModel
                    {
                        Name = deleteRequest.Name,
                    },
                    ErrorMessage = null
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "DeleteRole failed");
                return new FuncResponse<RoleDeleteResponseModel>
                {
                    ErrorMessage = "DeleteRole failed"
                };
            }
        }

        public async Task<FuncResponse<RoleGetResponseModel>> GetRole(RoleGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start GetRole w. param: {getRequest.ToJson()}");
                var foundRole = await _superCognitoDbContext
                    .Roles
                    .Where(x => x.Name == getRequest.Name)
                    .FirstOrDefaultAsync();
                if (foundRole == null)
                {
                    throw new Exception("role not found");
                }

                _logger.LogInformation("getRole success");
                return new FuncResponse<RoleGetResponseModel>
                {
                    Data = new RoleGetResponseModel
                    {
                        Id = foundRole.Id,
                        Name = foundRole.Name,
                        Description = foundRole.Description
                    },
                    ErrorMessage = null

                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "GetRole failed");
                return new FuncResponse<RoleGetResponseModel>
                {
                    ErrorMessage = "GetRole failed"
                };
            }
        }

        public async Task<FuncListResponse<RoleGetResponseModel>> ListRoles(RoleListRequestModel listRequest)
        {
            try
            {
                _logger.LogInformation($"Start ListRoles w. param: {listRequest.ToJson()}");
                var roles = _superCognitoDbContext.Roles.AsQueryable();

                if (!String.IsNullOrWhiteSpace(listRequest.Name))
                {
                    roles = roles.Where(x => x.Name.Contains(listRequest.Name));
                }

                roles = roles.OrderBy(x => x.Name);

                var total = await roles.CountAsync();
                var foundRoles = await roles
                    .Skip((int)((listRequest.Page - 1) * listRequest.PageSize))
                    .Take((int)listRequest.PageSize)
                    .Select(x => new RoleGetResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToListAsync();

                _logger.LogInformation("ListRoles success");
                return new FuncListResponse<RoleGetResponseModel>
                {
                    Data = foundRoles,
                    Total = total
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "ListRoles failed");
                return new FuncListResponse<RoleGetResponseModel>
                {
                    ErrorMessage = "ListRoles failed"
                };
            }
        }

        public async Task<FuncResponse<RoleUpdateResponseModel>> UpdateRole(RoleUpdateRequestModel updateRequest)
        {
            try
            {
                _logger.LogInformation($"Start UpdateRole w. data: {updateRequest.ToJson()}");
                var foundRole = await _superCognitoDbContext
                    .Roles
                    .Where(x => x.Name == updateRequest.Name)
                    .FirstOrDefaultAsync();
                if (foundRole == null)
                {
                    throw new Exception("Role for update not found");
                }

                foundRole.Description = updateRequest.Description;

                _superCognitoDbContext.Roles.Update(foundRole);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("UpdateRole success");
                return new FuncResponse<RoleUpdateResponseModel>
                {
                    Data = new RoleUpdateResponseModel
                    {
                        Id = foundRole.Id,
                        Name = foundRole.Name,
                        Description = foundRole.Description
                    },
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "updateRole failed");
                return new FuncResponse<RoleUpdateResponseModel>
                {
                    ErrorMessage = "updateRole failed"
                };
            }
        }
    }
}
