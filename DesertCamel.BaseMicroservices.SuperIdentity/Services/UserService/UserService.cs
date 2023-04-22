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
        private readonly SuperIdentityDbContext _superCognitoDbContext;
        private readonly ILogger<UserService> _logger;

        public UserService(
            SuperIdentityDbContext superCognitoDbContext,
            ILogger<UserService> logger)
        {
            _superCognitoDbContext = superCognitoDbContext;
            _logger = logger;
        }

        public FuncResponse<UserCreateResponseModel> Create(UserCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreateUser w. data: {createRequest.ToJson()}");
                var id = Guid.NewGuid();
                _superCognitoDbContext.Users.Add(new UserEntity
                {
                    Id = id,
                    PrincipalName = createRequest.PrincipalName,
                    UserPoolId = createRequest.UserPoolId,
                });
                _superCognitoDbContext.SaveChanges();

                _logger.LogInformation("creating user attributes");
                foreach (var attribute in createRequest.UserAttributes)
                {
                    if (attribute.Value == null)
                    {
                        continue;
                    }
                    var createUserAttributeResult = CreateAttribute(new UserAttributeCreateRequestModel
                    {
                        Key = attribute.Key,
                        UserId = id,
                        Value = attribute.Value,
                    });
                    if (createUserAttributeResult.IsError())
                    {
                        _logger.LogError($"failed to save attribute w. data: {attribute.ToJson()}");
                        continue;
                    }
                    _logger.LogInformation("success save attribute");
                }

                var getUserResult = Get(new UserGetRequestModel
                {
                    PrincipalName = createRequest.PrincipalName
                });
                if (getUserResult.IsError())
                {
                    throw new Exception("create user not saved successfully");
                }

                var createdUser = getUserResult.Data;

                _logger.LogInformation("create user success");
                return new FuncResponse<UserCreateResponseModel>
                {
                    Data = new UserCreateResponseModel
                    {
                        Id = createdUser.Id,
                        UserPoolId = createdUser.UserPoolId,
                        PrincipalName= createRequest.PrincipalName,
                        UserAttributes = createdUser.UserAttributes
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "create user failed");
                return new FuncResponse<UserCreateResponseModel>
                {
                    ErrorMessage = "create user"
                };
            }
        }

        public FuncResponse<UserAttributeUpdateResponseModel> UpdateAttribute(UserAttributeUpdateRequestModel updateRequest)
        {
            try
            {
                _logger.LogInformation($"Start UpdateAttribute w. data: {updateRequest.ToJson()}");
                var attribute = _superCognitoDbContext.UserAttributes
                    .Where(x => x.UserId.Equals(updateRequest.UserId) && x.Key.Equals(updateRequest.Key))
                    .FirstOrDefault();
                if (attribute == null)
                {
                    throw new Exception("attribute for update doesn't exist");
                }

                attribute.Value = updateRequest.Value;
                _superCognitoDbContext.UserAttributes.Update(attribute);
                _superCognitoDbContext.SaveChanges();

                _logger.LogInformation("success update attribute");
                return new FuncResponse<UserAttributeUpdateResponseModel>
                {
                    Data = new UserAttributeUpdateResponseModel
                    {
                        Id = attribute.Id,
                        UserId = updateRequest.UserId,
                        Key = updateRequest.Key,
                        Value = updateRequest.Value,
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to update attribute");
                return new FuncResponse<UserAttributeUpdateResponseModel>
                {
                    ErrorMessage = "failed to update attribute"
                };
            }
        }

        public FuncResponse<UserAttributeCreateResponseModel> CreateAttribute(UserAttributeCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start Add Attribute w. data: {createRequest.ToJson()}");
                var id = Guid.NewGuid();
                _superCognitoDbContext.UserAttributes.Add(new UserAttributeEntity
                {
                    Id = id,
                    Key = createRequest.Key,
                    UserId = createRequest.UserId,
                    Value = createRequest.Value
                });
                _superCognitoDbContext.SaveChanges();

                _logger.LogInformation("create user attribute success");
                return new FuncResponse<UserAttributeCreateResponseModel>
                {
                    Data = new UserAttributeCreateResponseModel
                    {
                        Id = id,
                        Key = createRequest.Key,
                        Value = createRequest.Value,
                        UserId = createRequest.UserId
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to create new user attribute");
                return new FuncResponse<UserAttributeCreateResponseModel>
                {
                    ErrorMessage = "failed to create user attribute"
                };
            }
        }

        public FuncResponse<UserGetResponseModel> Get(UserGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start GetUser w. data: {getRequest.ToJson()}");
                var user = _superCognitoDbContext.Users
                    .Include(x => x.UserAttributes)
                    .Where(x => x.PrincipalName.Equals(getRequest.PrincipalName))
                    .FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("get user not found");
                }

                _logger.LogInformation("get user success");
                return new FuncResponse<UserGetResponseModel>
                {
                    Data = new UserGetResponseModel
                    {
                        Id = user.Id,
                        PrincipalName = user.PrincipalName,
                        UserPoolId = user.UserPoolId,
                        UserAttributes = user.UserAttributes.ToDictionary(x => x.Key, x => new UserAttributeGetResponseModel
                        {
                            Id = x.Id,
                            Key = x.Key,
                            UserId = x.UserId,
                            Value = x.Value
                        })
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "GetUser failed");
                return new FuncResponse<UserGetResponseModel>
                {
                    ErrorMessage = "get user failed"
                };
            }
        }

        public FuncListResponse<UserGetResponseModel> List(UserListRequestModel listRequest)
        {
            try
            {
                _logger.LogInformation($"Start ListUsers w. data: {listRequest.ToJson()}");
                var userQuery = _superCognitoDbContext.Users
                    .Include(x => x.UserAttributes)
                    .AsQueryable();

                if (!String.IsNullOrWhiteSpace(listRequest.QuickSearch))
                {
                    userQuery = userQuery
                        .Where(x => x.PrincipalName.Contains(listRequest.QuickSearch))
                        .Where(x => x.UserAttributes.Any(x => x.Value.Contains(listRequest.QuickSearch)));
                }

                var count = userQuery.Count();

                var users = userQuery
                    .OrderBy(x => x.PrincipalName)
                    .Skip((int)(listRequest.PageSize * (listRequest.Page - 1)))
                    .Take((int)(listRequest.PageSize))
                    .ToList();

                _logger.LogInformation("success list users");
                return new FuncListResponse<UserGetResponseModel>
                {
                    Data = users.Select(x => new UserGetResponseModel
                    {
                        Id = x.Id,
                        PrincipalName = x.PrincipalName,
                        UserAttributes = x.UserAttributes.GroupBy(x => x.Key).Distinct().Select(x => x.First()).ToDictionary(x => x.Key, x => new UserAttributeGetResponseModel
                        {
                            Id = x.Id,
                            UserId = x.UserId,
                            Key = x.Key,
                            Value = x.Value
                        }),
                        UserPoolId = x.UserPoolId
                    }).ToList(),
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

        public FuncResponse<UserDeleteResponseModel> Delete(UserDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start DeleteUser w. data: {deleteRequest.ToJson()}");
                var user = _superCognitoDbContext.Users
                    .Where(x => x.PrincipalName.Equals(deleteRequest.PrincipalName))
                    .FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("user for deletion not found");
                }

                _superCognitoDbContext.Users.Remove(user);
                _superCognitoDbContext.SaveChanges();

                _logger.LogInformation("delete success");
                return new FuncResponse<UserDeleteResponseModel>
                {
                    Data = new UserDeleteResponseModel
                    {
                        PrincipalName = deleteRequest.PrincipalName
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "delete user failed");
                return new FuncResponse<UserDeleteResponseModel>
                {
                    ErrorMessage = "delete user failed"
                };
            }
        }
    }
}
