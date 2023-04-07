using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RolePermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.RolePermissionService
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly SuperIdentityDbContext _superIdentityDbContext;
        private readonly ILogger<RolePermissionService> _logger;

        public RolePermissionService(
            SuperIdentityDbContext superIdentityDbContext,
            ILogger<RolePermissionService> logger)
        {
            _superIdentityDbContext = superIdentityDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<RolePermissionCreateResponseModel>> Create(RolePermissionCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreateRolePermission w. data: {createRequest.ToJson()}");
                var newRolePermission = new RolePermissionEntity
                {
                    Id = $"{createRequest.RoleName}-{createRequest.PermissionName}",
                    PermissionName = createRequest.PermissionName,
                    RoleName = createRequest.RoleName,
                };

                _superIdentityDbContext.RolePermissions.Add(newRolePermission);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success create rolepermission");
                return new FuncResponse<RolePermissionCreateResponseModel>
                {
                    Data = new RolePermissionCreateResponseModel
                    {
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Failed to create role permission");
                return new FuncResponse<RolePermissionCreateResponseModel>
                {
                    ErrorMessage = "failed to create role permission"
                };
            }
        }

        public async Task<FuncResponse<RolePermissionDeleteResponseModel>> Delete(RolePermissionDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start DeleteRolePermission w. datA: {deleteRequest.ToJson()}");
                var foundRolePermission = await _superIdentityDbContext
                    .RolePermissions
                    .Where(x => x.RoleName.Equals(deleteRequest.RoleName) && x.PermissionName.Equals(deleteRequest.PermissionName))
                    .FirstOrDefaultAsync();
                if (foundRolePermission == null)
                {
                    throw new Exception("rolepermission for delete op not found");
                }

                _superIdentityDbContext.RolePermissions.Remove(foundRolePermission);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success delete role permissions");
                return new FuncResponse<RolePermissionDeleteResponseModel>
                {
                    Data = new RolePermissionDeleteResponseModel()
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to delete role permission");
                return new FuncResponse<RolePermissionDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete role permission"
                };
            }
        }

        public async Task<FuncResponse<RolePermissionGetResponseModel>> Get(RolePermissionGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start Get Role Permission w. data: {getRequest.ToJson()}");
                var foundRolePermission = await _superIdentityDbContext
                    .RolePermissions
                    .Where(x => x.RoleName.Equals(getRequest.RoleName) && x.PermissionName.Equals(getRequest.PermissionName))
                    .FirstOrDefaultAsync();
                if (foundRolePermission == null)
                {
                    throw new Exception("rolepermission for get op not found");
                }

                _logger.LogInformation("success rolepermission get");
                return new FuncResponse<RolePermissionGetResponseModel>
                {
                    Data = new RolePermissionGetResponseModel
                    {
                        RoleName = foundRolePermission.RoleName,
                        PermissionName = foundRolePermission.PermissionName,
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "fialed to get role permission");
                return new FuncResponse<RolePermissionGetResponseModel>
                {
                    ErrorMessage = "failed to get role permission"
                };
            }
        }

        public async Task<FuncListResponse<RolePermissionGetResponseModel>> List(RolePermissionListRequestModel listRequest)
        {
            try
            {
                _logger.LogInformation($"Start ListRolePermissions w. data: {listRequest.ToJson()}");
                var query = _superIdentityDbContext
                    .RolePermissions
                    .Include(x => x.Permission)
                    .AsQueryable();
                query = query.Where(x => x.RoleName == listRequest.RoleName);

                if (!String.IsNullOrWhiteSpace(listRequest.PermissionName))
                {
                    query = query.Where(x => x.PermissionName.Contains(listRequest.PermissionName));
                }

                if (!String.IsNullOrWhiteSpace(listRequest.RoleName))
                {
                    query = query.OrderBy(x => x.PermissionName);
                } 
                else 
                {
                    query = query.OrderBy(x => x.Id);
                }

                var total = await query.CountAsync();
                var results = await query
                    .Skip((listRequest.Page - 1) * listRequest.PageSize)
                    .Take(listRequest.PageSize)
                    .Select(x => new RolePermissionGetResponseModel
                    {
                        RoleName = x.RoleName,
                        PermissionName = x.PermissionName,
                        Description = x.Permission.Description
                    })
                    .ToListAsync();

                _logger.LogInformation("success list role permissions");
                return new FuncListResponse<RolePermissionGetResponseModel>
                {
                    Total = total,
                    Data = results
                };
                
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to list role permissions");
                return new FuncListResponse<RolePermissionGetResponseModel>
                {
                    ErrorMessage = "failed to list role permission"
                };
            }
        }

        public async Task<FuncListResponse<RolePermissionOptionsResponseModel>> Options(RolePermissionOptionsRequestModel optionsRequest)
        {
            try
            {
                _logger.LogInformation($"Start List Permission Options w. data: {optionsRequest.ToJson()}");
                var query = _superIdentityDbContext
                    .Permissions
                    .Include(x => x.PermissionRoles)
                    .AsQueryable();
                query = query.Where(x => !x.PermissionRoles.Where(x => x.RoleName.Equals(optionsRequest.RoleName)).Any());
                if (!String.IsNullOrWhiteSpace(optionsRequest.PermissionName))
                {
                    query = query.Where(x => x.Name.Contains(optionsRequest.PermissionName));
                }

                var count = await query.CountAsync();
                var foundPermissions = await query
                    .OrderBy(x => x.Name)
                    .Skip(optionsRequest.PageSize * (optionsRequest.Page - 1))
                    .Take(optionsRequest.PageSize)
                    .Select(x => new RolePermissionOptionsResponseModel
                    {
                        PermissionName = x.Name,
                        Description = x.Description
                    })
                    .ToListAsync();

                _logger.LogInformation("success list permission options");
                return new FuncListResponse<RolePermissionOptionsResponseModel>
                {
                    Total = count,
                    Data = foundPermissions,
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to list permission options");
                return new FuncListResponse<RolePermissionOptionsResponseModel>
                {
                    ErrorMessage = "failed to list permission options"
                };
            }
        }
    }
}
