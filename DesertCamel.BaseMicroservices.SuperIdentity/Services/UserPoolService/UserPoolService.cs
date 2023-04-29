using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserPoolService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.UserPoolService
{
    public class UserPoolService : IUserPoolService
    {
        private readonly SuperIdentityDbContext _superCognitoDbContext;
        private readonly ILogger<UserPoolService> _logger;

        public UserPoolService(
            SuperIdentityDbContext superCognitoDbContext,
            ILogger<UserPoolService> logger)
        {
            this._superCognitoDbContext = superCognitoDbContext;
            this._logger = logger;
        }

        public async Task<FuncResponse<UserPoolCreateResponseModel>> CreateUserPool(UserPoolCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreateUserPool w. data: {createRequest.ToJson()}");
                var newUserPool = new UserPoolEntity
                {
                    Id = Guid.NewGuid(),
                    Name = createRequest.Name,
                    Description = createRequest.Description,
                    ClientId = createRequest.ClientId,
                    ClientSecret = createRequest.ClientSecret,
                    Enabled = createRequest.Enabled,
                    UseCache = createRequest.UseCache,
                    ExchangeTokenUrl = createRequest.ExchangeTokenUrl,
                    IssuerUrl = createRequest.IssuerUrl,
                    JwksUrl = createRequest.JwksUrl,
                    LoginPageUrl = createRequest.LoginPageUrl,
                    UserInfoUrl = createRequest.UserInfoUrl,
                    PrincipalNameKey = createRequest.PrincipalNameKey,
                    TokenLifeTime = createRequest.TokenLifeTime,
                    RedirectUri = createRequest.RedirectUri,
                    ApplicationCallbackUrl = createRequest.ApplicationCallbackUrl
                };

                await _superCognitoDbContext.UserPools.AddAsync(newUserPool);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("CreateUserPoolAPI success");
                return new FuncResponse<UserPoolCreateResponseModel>
                {
                    Data = new UserPoolCreateResponseModel(),
                    ErrorMessage = null
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "CreateUserPoolAPI failed");
                return new FuncResponse<UserPoolCreateResponseModel>
                {
                    ErrorMessage = "CreateUserPoolAPI failed"
                };
            }
        }

        public async Task<FuncResponse<UserPoolDeleteResponseModel>> DeleteUserPool(UserPoolDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start DeleteUserPool w. param: {deleteRequest.ToJson()}");
                var userPool = _superCognitoDbContext
                    .UserPools
                    .Where(x => x.Id == deleteRequest.UserPoolId)
                    .FirstOrDefault();
                if (userPool == null)
                {
                    throw new Exception("UserPool Not found");
                }

                _superCognitoDbContext.UserPools.Remove(userPool);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("DeleteUserPool success");
                return new FuncResponse<UserPoolDeleteResponseModel>
                {
                    Data = new UserPoolDeleteResponseModel
                    {
                        UserPoolId = deleteRequest.UserPoolId,
                    },
                    ErrorMessage = null
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "DeleteUserPool failed");
                return new FuncResponse<UserPoolDeleteResponseModel>
                {
                    ErrorMessage = "DeleteUserPool failed"
                };
            }
        }

        public async Task<FuncResponse<UserPoolGetResponseModel>> GetUserPool(UserPoolGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start GetUserPool w. param: {getRequest.ToJson()}");
                var userPool = _superCognitoDbContext
                    .UserPools
                    .Where(x => x.Id == getRequest.UserPoolId)
                    .FirstOrDefault();
                if (userPool == null)
                {
                    throw new Exception("UserPool Not found");
                }

                _logger.LogInformation("GetUserPoolAPI success");
                return new FuncResponse<UserPoolGetResponseModel>
                {
                    Data = new UserPoolGetResponseModel
                    {
                        Id = userPool.Id,
                        Name = userPool.Name,
                        ClientId = userPool.ClientId,
                        ClientSecret = userPool.ClientSecret,
                        Description = userPool.Description,
                        Enabled = userPool.Enabled,
                        UseCache = userPool.UseCache,
                        ExchangeTokenUrl = userPool.ExchangeTokenUrl,
                        IssuerUrl = userPool.IssuerUrl,
                        JwksUrl = userPool.JwksUrl,
                        LoginPageUrl = userPool.LoginPageUrl,
                        PrincipalNameKey = userPool.PrincipalNameKey,
                        TokenLifeTime = userPool.TokenLifeTime,
                        UserInfoUrl = userPool.UserInfoUrl,
                        ApplicationCallbackUrl = userPool.ApplicationCallbackUrl,
                        RedirectUri = userPool.RedirectUri
                    },
                    ErrorMessage = null
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "GetUserPoolAPI failed");
                return new FuncResponse<UserPoolGetResponseModel>
                {
                    ErrorMessage = "GetUserPoolAPI failed"
                };
            }
        }

        public async Task<FuncListResponse<UserPoolListActiveItemResponseModel>> ListActiveUserPools(UserPoolListActiveRequestModel listActiveRequest)
        {
            try
            {
                _logger.LogInformation($"Start ListActiveUserPools w. param: {listActiveRequest.ToJson()}");
                var results = await _superCognitoDbContext.UserPools
                    .Where(x => x.Enabled)
                    .Select(x => new UserPoolListActiveItemResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        LoginPageUrl = x.LoginPageUrl
                    })
                    .ToListAsync();

                _logger.LogInformation("success list active user pools");
                return new FuncListResponse<UserPoolListActiveItemResponseModel>
                {
                    Data = results,
                    Total = results.Count
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "ListActiveUserPools failed");
                return new FuncListResponse<UserPoolListActiveItemResponseModel>
                {
                    ErrorMessage = "ListActiveUserPools failed"
                };
            }
        }

        public async Task<FuncListResponse<UserPoolGetResponseModel>> ListUserPools(UserPoolListRequestModel listRequest)
        {
            try
            {
                _logger.LogInformation($"Start ListUserPool w. param: {listRequest.ToJson()}");
                var query = _superCognitoDbContext.UserPools.AsQueryable();

                if (!String.IsNullOrWhiteSpace(listRequest.Name))
                {
                    query = query
                        .Where(x => x.Name.Contains(listRequest.Name));
                }

                query = query.OrderBy(x => x.Name);
                var total = await query.CountAsync();

                var userPools = await query
                    .Skip((int)(listRequest.PageSize * (listRequest.Page - 1)))
                    .Take((int)listRequest.PageSize)
                    .Select(x => new UserPoolGetResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ClientId = x.ClientId,
                        ClientSecret = x.ClientSecret,
                        Description = x.Description,
                        Enabled = x.Enabled,
                        UseCache = x.UseCache,
                        ExchangeTokenUrl = x.ExchangeTokenUrl,
                        IssuerUrl = x.IssuerUrl,
                        JwksUrl = x.JwksUrl,
                        LoginPageUrl = x.LoginPageUrl,
                        PrincipalNameKey = x.PrincipalNameKey,
                        TokenLifeTime = x.TokenLifeTime,
                        UserInfoUrl = x.UserInfoUrl,
                        RedirectUri = x.RedirectUri,
                        ApplicationCallbackUrl = x.ApplicationCallbackUrl
                    })
                    .ToListAsync();

                _logger.LogInformation("ListUserPool success");
                return new FuncListResponse<UserPoolGetResponseModel>
                {
                    Data = userPools,
                    Total = total
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "ListUserPool failed");
                return new FuncListResponse<UserPoolGetResponseModel>
                {
                    ErrorMessage = "ListUserPool failed"
                };
            }
        }

        public async Task<FuncResponse<UserPoolUpdateResponseModel>> UpdateUserPool(UserPoolUpdateRequestModel updateRequest)
        {
            try
            {
                _logger.LogInformation($"Start UpdateUserPool w. param: {updateRequest.ToJson()}");
                var userPool = await _superCognitoDbContext
                    .UserPools
                    .Where(x => x.Id == updateRequest.UserPoolId)
                    .FirstOrDefaultAsync();

                if (userPool == null)
                {
                    throw new Exception("UserPool not found");
                }

                userPool.Name = updateRequest.Name;
                userPool.ClientId = updateRequest.ClientId;
                userPool.ClientSecret = updateRequest.ClientSecret;
                userPool.Description = updateRequest.Description;
                userPool.Enabled = updateRequest.Enabled;
                userPool.UseCache = updateRequest.UseCache;
                userPool.ExchangeTokenUrl = updateRequest.ExchangeTokenUrl;
                userPool.IssuerUrl = updateRequest.IssuerUrl;
                userPool.JwksUrl = updateRequest.JwksUrl;
                userPool.LoginPageUrl = updateRequest.LoginPageUrl;
                userPool.PrincipalNameKey = updateRequest.PrincipalNameKey;
                userPool.TokenLifeTime = updateRequest.TokenLifeTime;
                userPool.UserInfoUrl = updateRequest.UserInfoUrl;
                userPool.ApplicationCallbackUrl = updateRequest.ApplicationCallbackUrl;
                userPool.RedirectUri = updateRequest.RedirectUri;

                _superCognitoDbContext.UserPools.Update(userPool);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("UpdateUserPool success");
                return new FuncResponse<UserPoolUpdateResponseModel>
                {
                    Data = new UserPoolUpdateResponseModel
                    {
                        Id = userPool.Id,
                        Name = userPool.Name,
                        ClientId = userPool.ClientId,
                        ClientSecret = userPool.ClientSecret,
                        Description = userPool.Description,
                        Enabled = userPool.Enabled,
                        UseCache = userPool.UseCache,
                        ExchangeTokenUrl = userPool.ExchangeTokenUrl,
                        IssuerUrl = userPool.IssuerUrl,
                        JwksUrl = userPool.JwksUrl,
                        LoginPageUrl = userPool.LoginPageUrl,
                        PrincipalNameKey = userPool.PrincipalNameKey,
                        TokenLifeTime = userPool.TokenLifeTime,
                        UserInfoUrl = userPool.UserInfoUrl,
                    },
                    ErrorMessage = null
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "UpdateUserPoolAPI failed");
                return new FuncResponse<UserPoolUpdateResponseModel>
                {
                    ErrorMessage = "UpdateUserPoolAPI failed"
                };
            }
        }
    }
}
