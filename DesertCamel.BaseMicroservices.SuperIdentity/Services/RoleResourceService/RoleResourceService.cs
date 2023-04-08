using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleResourceService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.RoleResourceService
{
    public class RoleResourceService : IRoleResourceService
    {
        private readonly SuperIdentityDbContext _superIdentityDbContext;
        private readonly ILogger<RoleResourceService> _logger;

        public RoleResourceService(
            SuperIdentityDbContext superIdentityDbContext,
            ILogger<RoleResourceService> logger)
        {
            _superIdentityDbContext = superIdentityDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<RoleResourceCreateResponseModel>> Create(RoleResourceCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreateRoleResource w. data: {createRequest.ToJson()}");
                var newRoleResource = new RoleResourceEntity
                {
                    Id = $"{createRequest.RoleName}-{createRequest.ResourceName}",
                    ResourceName = createRequest.ResourceName,
                    RoleName = createRequest.RoleName,
                };

                _superIdentityDbContext.RoleResources.Add(newRoleResource);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success create roleresource");
                return new FuncResponse<RoleResourceCreateResponseModel>
                {
                    Data = new RoleResourceCreateResponseModel
                    {
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to create role resource");
                return new FuncResponse<RoleResourceCreateResponseModel>
                {
                    ErrorMessage = "failed to create role resource"
                };
            }
        }

        public async Task<FuncResponse<RoleResourceDeleteResponseModel>> Delete(RoleResourceDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start DeleteRoleResource w. data: {deleteRequest.ToJson()}");
                var foundRoleResource = await _superIdentityDbContext
                    .RoleResources
                    .Where(x => x.RoleName.Equals(deleteRequest.RoleName) && x.ResourceName.Equals(deleteRequest.ResourceName))
                    .FirstOrDefaultAsync();
                if (foundRoleResource == null)
                {
                    throw new Exception("roleresource for delete op not found");
                }

                _superIdentityDbContext.RoleResources.Remove(foundRoleResource);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success delete role permissions");
                return new FuncResponse<RoleResourceDeleteResponseModel>
                {
                    Data = new RoleResourceDeleteResponseModel()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to delete role resource");
                return new FuncResponse<RoleResourceDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete role resource"
                };
            }
        }

        public async Task<FuncResponse<RoleResourceGetResponseModel>> Get(RoleResourceGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start Get Role Resource w. data: {getRequest.ToJson()}");
                var foundRoleResource = await _superIdentityDbContext
                    .RoleResources
                    .Where(x => x.RoleName.Equals(getRequest.RoleName) && x.ResourceName.Equals(getRequest.ResourceName))
                    .FirstOrDefaultAsync();
                if (foundRoleResource == null)
                {
                    throw new Exception("roleresource for get op not found");
                }

                _logger.LogInformation("success roleresource get");
                return new FuncResponse<RoleResourceGetResponseModel>
                {
                    Data = new RoleResourceGetResponseModel
                    {
                        RoleName = foundRoleResource.RoleName,
                        ResourceName = foundRoleResource.ResourceName,
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to get roleresource");
                return new FuncResponse<RoleResourceGetResponseModel>
                {
                    ErrorMessage = "failed to get roleresource"
                };
            }
        }

        public async Task<FuncListResponse<RoleResourceGetResponseModel>> List(RoleResourceListRequestModel listRequest)
        {
            try
            {
                _logger.LogInformation($"Start ListRoleResources w. data: {listRequest.ToJson()}");
                var query = _superIdentityDbContext
                    .RoleResources
                    .Include(x => x.Resource)
                    .AsQueryable();
                query = query.Where(x => x.RoleName == listRequest.RoleName);

                if (!String.IsNullOrWhiteSpace(listRequest.ResourceName))
                {
                    query = query.Where(x => x.ResourceName.Contains(listRequest.ResourceName));
                }

                if (!String.IsNullOrWhiteSpace(listRequest.RoleName))
                {
                    query = query.OrderBy(x => x.ResourceName);
                }
                else
                {
                    query = query.OrderBy(x => x.Id);
                }

                var total = await query.CountAsync();
                var results = await query
                    .Skip((listRequest.Page - 1) * listRequest.PageSize)
                    .Take(listRequest.PageSize)
                    .Select(x => new RoleResourceGetResponseModel
                    {
                        RoleName = x.RoleName,
                        ResourceName = x.ResourceName,
                        Description = x.Resource.Description
                    })
                    .ToListAsync();

                _logger.LogInformation("success ListRoleResources");
                return new FuncListResponse<RoleResourceGetResponseModel>
                {
                    Total = total,
                    Data = results
                };

            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to ListRoleResources");
                return new FuncListResponse<RoleResourceGetResponseModel>
                {
                    ErrorMessage = "failed to ListRoleResources"
                };
            }
        }

        public async Task<FuncListResponse<RoleResourceOptionsResponseModel>> Options(RoleResourceOptionsRequestModel optionsRequest)
        {
            try
            {
                _logger.LogInformation($"Start List Resource Options w. data: {optionsRequest.ToJson()}");
                var query = _superIdentityDbContext
                    .Resources
                    .Include(x => x.ResourceRoles)
                    .AsQueryable();
                query = query.Where(x => !x.ResourceRoles.Where(x => x.RoleName.Equals(optionsRequest.RoleName)).Any());
                if (!String.IsNullOrWhiteSpace(optionsRequest.ResourceName))
                {
                    query = query.Where(x => x.Name.Contains(optionsRequest.ResourceName));
                }

                var count = await query.CountAsync();
                var foundPermissions = await query
                    .OrderBy(x => x.Name)
                    .Skip(optionsRequest.PageSize * (optionsRequest.Page - 1))
                    .Take(optionsRequest.PageSize)
                    .Select(x => new RoleResourceOptionsResponseModel
                    {
                        ResourceName = x.Name,
                        Description = x.Description
                    })
                    .ToListAsync();

                _logger.LogInformation("success list resource options");
                return new FuncListResponse<RoleResourceOptionsResponseModel>
                {
                    Total = count,
                    Data = foundPermissions,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to list resource options");
                return new FuncListResponse<RoleResourceOptionsResponseModel>
                {
                    ErrorMessage = "failed to list resource options"
                };
            }
        }
    }
}
