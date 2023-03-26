using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.PermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.PermissionService
{
    public class PermissionService : IPermissionService
    {
        private readonly SuperCognitoDbContext _superCognitoDbContext;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(
            SuperCognitoDbContext superCognitoDbContext,
            ILogger<PermissionService> logger)
        {
            _superCognitoDbContext = superCognitoDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<PermissionCreateResponseModel>> Create(PermissionCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreatePermission w. data: {createRequest.ToJson()}");
                var newPermission = new PermissionEntity
                {
                    Id = Guid.NewGuid(),
                    Name = createRequest.Name,
                    Description = createRequest.Description
                };

                _superCognitoDbContext.Permissions.Add(newPermission);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("success create permission");
                return new FuncResponse<PermissionCreateResponseModel>
                {
                    Data = new PermissionCreateResponseModel
                    {
                        Id = newPermission.Id,
                        Name = newPermission.Name,
                        Description = newPermission.Description
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to create permission");
                return new FuncResponse<PermissionCreateResponseModel>
                {
                    ErrorMessage = "failed to create permission"
                };
            }
        }

        public async Task<FuncResponse<PermissionDeleteResponseModel>> Delete(PermissionDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start PermissionDelete w. data: {deleteRequest.ToJson()}");
                var foundPermission = await _superCognitoDbContext
                    .Permissions
                    .Where(x => x.Name.Equals(deleteRequest.Name))
                    .FirstOrDefaultAsync();
                if (foundPermission == null)
                {
                    throw new Exception("Permission for delete op is not found");
                }

                _superCognitoDbContext.Permissions.Remove(foundPermission);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("success deletePermission");
                return new FuncResponse<PermissionDeleteResponseModel>
                {
                    Data = new PermissionDeleteResponseModel()
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to delete permission");
                return new FuncResponse<PermissionDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete permission"
                };
            }
        }

        public async Task<FuncResponse<PermissionGetResponseModel>> Get(PermissionGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start GetPermission w. data: {getRequest.ToJson()}");
                var foundPermission = await _superCognitoDbContext
                    .Permissions
                    .Where(x => x.Name.Equals(getRequest.Name))
                    .FirstOrDefaultAsync();
                if (foundPermission == null)
                {
                    throw new Exception("permission for get op is not found");
                }

                _logger.LogInformation("success get permission");
                return new FuncResponse<PermissionGetResponseModel>
                {
                    Data = new PermissionGetResponseModel
                    {
                        Id = foundPermission.Id,
                        Name = foundPermission.Name,
                        Description = foundPermission.Description,
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to get permission");
                return new FuncResponse<PermissionGetResponseModel>
                {
                    ErrorMessage = "failed to get permission"
                };
            }
        }

        public async Task<FuncListResponse<PermissionGetResponseModel>> List(PermissionListRequestModel listRequest)
        {
            try
            {
                _logger.LogInformation($"Start listPermission w. data: {listRequest.ToJson()}");
                var query = _superCognitoDbContext.Permissions.AsQueryable();

                if (!String.IsNullOrWhiteSpace(listRequest.Name))
                {
                    query = query.Where(x => x.Name.Contains(listRequest.Name));
                }

                var total = await query.CountAsync();
                
                query = query.OrderBy(x => x.Name);

                var foundPermissions = await query
                    .Skip((int) listRequest.PageSize * ((int) listRequest.Page - 1))
                    .Take((int) listRequest.PageSize)
                    .Select(x => new PermissionGetResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description=x.Description,
                    })
                    .ToListAsync();

                _logger.LogInformation("success list permissions");
                return new FuncListResponse<PermissionGetResponseModel>
                {
                    Total = total,
                    Data = foundPermissions,
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to list permissions");
                return new FuncListResponse<PermissionGetResponseModel>
                {
                    ErrorMessage = "failed to list permissions"
                };
            }
        }

        public async Task<FuncResponse<PermissionUpdateResponseModel>> Update(PermissionUpdateRequestModel updateRequest)
        {
            try
            {
                _logger.LogInformation($"Start UpdatePermission w. data: {updateRequest.ToJson()}");
                var foundPermission = _superCognitoDbContext
                    .Permissions
                    .Where(x => x.Name.Equals(updateRequest.Name))
                    .FirstOrDefault();
                if (foundPermission == null)
                {
                    throw new Exception("permission for update op not found");
                }

                foundPermission.Description = updateRequest.Description;
                _superCognitoDbContext.Permissions.Update(foundPermission);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("success update permission");
                return new FuncResponse<PermissionUpdateResponseModel>
                {
                    Data = new PermissionUpdateResponseModel
                    {
                        Id = foundPermission.Id,
                        Name = foundPermission.Name,
                        Description = updateRequest.Description
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Failed to update permission");
                return new FuncResponse<PermissionUpdateResponseModel>
                {
                    ErrorMessage = "failed to update permission"
                };
            }
        }
    }
}
