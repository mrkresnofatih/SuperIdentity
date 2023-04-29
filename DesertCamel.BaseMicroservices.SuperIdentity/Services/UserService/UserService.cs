using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly SuperIdentityDbContext _superIdentityDbContext;
        private readonly ILogger<UserService> _logger;

        public UserService(
            SuperIdentityDbContext superIdentityDbContext,
            ILogger<UserService> logger)
        {
            _superIdentityDbContext = superIdentityDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<UserCreateResponseModel>> Create(UserCreateRequestModel createRequest)
        {
            _logger.LogInformation($"Start CreateUser w. data: {createRequest.ToJson()}");
            try
            {
                var newUser = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    UserPoolId = createRequest.UserPoolId,
                    PrincipalName = createRequest.PrincipalName
                };
                _superIdentityDbContext.Users.Add(newUser);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success: createUser");
                return new FuncResponse<UserCreateResponseModel>
                {
                    Data = new UserCreateResponseModel
                    {
                        Id = newUser.Id,
                        PrincipalName = newUser.PrincipalName,
                        UserPoolId = newUser.UserPoolId
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to create user");
                return new FuncResponse<UserCreateResponseModel>
                {
                    ErrorMessage = "failed to create user"
                };
            }
        }

        public async Task<FuncResponse<UserAttributeCreateResponseModel>> CreateAttribute(UserAttributeCreateRequestModel createRequest)
        {
            _logger.LogInformation($"Start CreateAttribute w. data: {createRequest.ToJson()}");
            try
            {
                var newAttribute = new UserAttributeEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = createRequest.UserId,
                    Key = createRequest.Key,
                    Value = createRequest.Value,
                };
                _superIdentityDbContext.UserAttributes.Add(newAttribute);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success: createAttribute");
                return new FuncResponse<UserAttributeCreateResponseModel>
                {
                    Data = new UserAttributeCreateResponseModel
                    {
                        Id = newAttribute.Id,
                        Key = newAttribute.Key,
                        Value = newAttribute.Value,
                        UserId = newAttribute.UserId
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to create user attribute");
                return new FuncResponse<UserAttributeCreateResponseModel>
                {
                    ErrorMessage = "failed to create user attribute"
                };
            }
        }

        public async Task<FuncResponse<UserDeleteResponseModel>> Delete(UserDeleteRequestModel deleteRequest)
        {
            _logger.LogInformation($"start delete user w. data: {deleteRequest.ToJson()}");
            try
            {
                var foundUser = await _superIdentityDbContext.Users
                    .Where(x => x.PrincipalName.Equals(deleteRequest.PrincipalName))
                    .FirstOrDefaultAsync();
                if (foundUser == null)
                {
                    throw new Exception("user for delete op not found");
                }

                _superIdentityDbContext.Users.Remove(foundUser);
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success delete user");
                return new FuncResponse<UserDeleteResponseModel>
                {
                    Data = new UserDeleteResponseModel
                    {
                        PrincipalName = foundUser.PrincipalName,
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to delete user");
                return new FuncResponse<UserDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete user"
                };
            }
        }

        public async Task<FuncResponse<UserGetResponseModel>> Get(UserGetRequestModel deleteRequest)
        {
            _logger.LogInformation($"start get user w. data: {deleteRequest.ToJson()}");
            try
            {
                var foundUser = await _superIdentityDbContext.Users
                    .Include(x => x.UserAttributes)
                    .Where(x => x.PrincipalName.Equals(deleteRequest.PrincipalName))
                    .Select(x => new UserGetResponseModel
                    {
                        Id = x.Id,
                        PrincipalName = x.PrincipalName,
                        UserPoolId = x.UserPoolId,
                        UserAttributes = x.UserAttributes
                            .Select(x => new UserAttributeGetResponseModel
                            {
                                Id = x.Id,
                                Key = x.Key,
                                Value = x.Value,
                                UserId = x.UserId
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();
                if (foundUser == null)
                {
                    throw new Exception("user for get op not found");
                }

                _logger.LogInformation("success get user");
                return new FuncResponse<UserGetResponseModel>
                {
                    Data = foundUser,                    
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to get user");
                return new FuncResponse<UserGetResponseModel>
                {
                    ErrorMessage = "failed to get user"
                };
            }
        }

        public async Task<FuncListResponse<UserGetResponseModel>> List(UserListRequestModel listRequest)
        {
            _logger.LogInformation($"Start List w. data: {listRequest.ToJson()}");
            try
            {
                var query = _superIdentityDbContext
                    .Users
                    .Include(x => x.UserAttributes)
                    .AsQueryable();
                
                if (!String.IsNullOrWhiteSpace(listRequest.PrincipalName))
                {
                    query = query.Where(x => x.PrincipalName.Contains(listRequest.PrincipalName));
                }

                query = query.OrderBy(x => x.PrincipalName);
                var count = await query.CountAsync();
                var users = await query
                    .Skip((listRequest.Page - 1) * listRequest.PageSize)
                    .Take(listRequest.PageSize)
                    .Select(x => new UserGetResponseModel
                    {
                        Id = x.Id,
                        PrincipalName = x.PrincipalName,
                        UserPoolId = x.UserPoolId,
                        UserAttributes = x.UserAttributes
                            .Select(x => new UserAttributeGetResponseModel
                            {
                                Id = x.Id,
                                Key = x.Key,
                                Value = x.Value,
                                UserId = x.UserId
                            })
                            .ToList()
                    })
                    .ToListAsync();

                _logger.LogInformation("success: list users");
                return new FuncListResponse<UserGetResponseModel>
                {
                    Data = users,
                    Total = count
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to list users");
                return new FuncListResponse<UserGetResponseModel>
                {
                    ErrorMessage = "failed to list users"
                };
            }
        }

        public async Task<FuncResponse<UserAttributeUpdateResponseModel>> UpdateAttribute(UserAttributeUpdateRequestModel updateRequest)
        {
            _logger.LogInformation($"Start UpdateAttribute w. data: {updateRequest.ToJson()}");
            try
            {
                var foundAttribute = await _superIdentityDbContext.UserAttributes
                    .Where(x => x.UserId.Equals(updateRequest.UserId) && x.Key.Equals(updateRequest.Key))
                    .FirstOrDefaultAsync();
                if (foundAttribute == null)
                {
                    throw new Exception("user attribute for update op not found");
                }

                foundAttribute.Value = updateRequest.Value;
                await _superIdentityDbContext.SaveChangesAsync();

                _logger.LogInformation("success: updateAttribute");
                return new FuncResponse<UserAttributeUpdateResponseModel>
                {
                    Data = new UserAttributeUpdateResponseModel
                    {
                        Id = foundAttribute.Id,
                        Key = foundAttribute.Key,
                        Value = foundAttribute.Value,
                        UserId = foundAttribute.UserId
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to update user attribute");
                return new FuncResponse<UserAttributeUpdateResponseModel>
                {
                    ErrorMessage = "failed to update user attribute"
                };
            }
        }
    }
}
