using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthorityService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.UserAuthorityService
{
    public class UserAuthorityService : IUserAuthorityService
    {
        private readonly SuperIdentityDbContext _superIdentityDbContext;
        private readonly ILogger<UserAuthorityService> _logger;

        public UserAuthorityService(
            SuperIdentityDbContext superIdentityDbContext,
            ILogger<UserAuthorityService> logger)
        {
            _superIdentityDbContext = superIdentityDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<UserAuthorityAddResponseModel>> Add(UserAuthorityAddRequestModel createRequest)
        {
            _logger.LogInformation($"Start AddUserAuthority w. data: {createRequest.ToJson()}");
            try
            {
                var foundUserAuthority = await _superIdentityDbContext
                    .UserAuthorities
                    .Where(x => x.PrincipalName.Equals(createRequest.PrincipalName) && x.RoleResourceId.Equals(createRequest.RoleResourceId))
                    .FirstOrDefaultAsync();
                if (foundUserAuthority != null)
                {
                    _logger.LogInformation("client authority w. client-name & role-resource-id already exists");
                    return new FuncResponse<UserAuthorityAddResponseModel>
                    {
                        Data = new UserAuthorityAddResponseModel
                        {
                            Id = foundUserAuthority.Id,
                            PrincipalName = foundUserAuthority.PrincipalName,
                            RoleResourceId = foundUserAuthority.RoleResourceId
                        }
                    };
                }

                var newUserAuthority = new UserAuthorityEntity
                {
                    Id = Guid.NewGuid(),
                    PrincipalName = createRequest.PrincipalName,
                    RoleResourceId = createRequest.RoleResourceId
                };
                _superIdentityDbContext.UserAuthorities.Add(newUserAuthority);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("user-authority add success");
                return new FuncResponse<UserAuthorityAddResponseModel>
                {
                    Data = new UserAuthorityAddResponseModel
                    {
                        Id = newUserAuthority.Id,
                        PrincipalName = newUserAuthority.PrincipalName,
                        RoleResourceId = newUserAuthority.RoleResourceId
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to add user authority");
                return new FuncResponse<UserAuthorityAddResponseModel>
                {
                    ErrorMessage = "failed to add user authority"
                };
            }
        }

        public async Task<FuncResponse<UserAuthorityDeleteResponseModel>> Delete(UserAuthorityDeleteRequestModel deleteRequest)
        {
            _logger.LogInformation($"Start DeleteUserAuthority w. data: {deleteRequest.ToJson()}");
            try
            {
                var foundUserAuthority = await _superIdentityDbContext
                    .UserAuthorities
                    .Where(x => x.PrincipalName.Equals(deleteRequest.PrincipalName) && x.RoleResourceId.Equals(deleteRequest.RoleResourceId))
                    .FirstOrDefaultAsync();
                if (foundUserAuthority == null)
                {
                    _logger.LogInformation("client authority w. client-name & role-resource-id already deleted");
                    return new FuncResponse<UserAuthorityDeleteResponseModel>
                    {
                        Data = new UserAuthorityDeleteResponseModel()
                    };
                }

                _superIdentityDbContext.UserAuthorities.Remove(foundUserAuthority);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("client-authority delete success");
                return new FuncResponse<UserAuthorityDeleteResponseModel>
                {
                    Data = new UserAuthorityDeleteResponseModel()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to delete user authority");
                return new FuncResponse<UserAuthorityDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete user authority"
                };
            }
        }

        public async Task<FuncResponse<UserAuthorityGetResponseModel>> Get(UserAuthorityGetRequestModel getRequest)
        {
            _logger.LogInformation($"Start GetUserAuthority w. data: {getRequest.ToJson()}");
            try
            {
                var foundClientAuthority = await _superIdentityDbContext
                    .UserAuthorities
                    .Where(x => x.PrincipalName.Equals(getRequest.PrincipalName) && x.RoleResourceId.Equals(getRequest.RoleResourceId))
                    .FirstOrDefaultAsync();
                if (foundClientAuthority != null)
                {
                    _logger.LogInformation("get user authority w. principal-name & role-resource-id success");
                    return new FuncResponse<UserAuthorityGetResponseModel>
                    {
                        Data = new UserAuthorityGetResponseModel
                        {
                            PrincipalName = foundClientAuthority.PrincipalName,
                            RoleResourceId = foundClientAuthority.RoleResourceId,
                            Id = foundClientAuthority.Id
                        }
                    };
                }

                throw new Exception("User-Authority not found");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to get user authority");
                return new FuncResponse<UserAuthorityGetResponseModel>
                {
                    ErrorMessage = "failed to get user authority"
                };
            }
        }

        public async Task<FuncListResponse<UserAuthorityGetResponseModel>> List(UserAuthorityListRequestModel listRequest)
        {
            _logger.LogInformation($"Start ListUserAuthority w. data: {listRequest.ToJson()}");
            try
            {
                var query = _superIdentityDbContext
                    .UserAuthorities
                    .Where(x => x.PrincipalName.Equals(listRequest.PrincipalName));
                var total = await query.CountAsync();
                var foundUserAuthorities = await query
                    .OrderBy(x => x.RoleResourceId)
                    .Skip(listRequest.PageSize * (listRequest.Page - 1))
                    .Take(listRequest.PageSize)
                    .Select(x => new UserAuthorityGetResponseModel
                    {
                        Id = x.Id,
                        PrincipalName = x.PrincipalName,
                        RoleResourceId = x.RoleResourceId
                    })
                    .ToListAsync();

                _logger.LogInformation("success: listUserAuthority");
                return new FuncListResponse<UserAuthorityGetResponseModel>
                {
                    Data = foundUserAuthorities,
                    Total = total,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to list user authority");
                return new FuncListResponse<UserAuthorityGetResponseModel>
                {
                    ErrorMessage = "failed to list user authority"
                };
            }
        }
    }
}
