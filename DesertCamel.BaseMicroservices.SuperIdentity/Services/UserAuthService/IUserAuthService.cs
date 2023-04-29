using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.UserAuthService
{
    public interface IUserAuthService
    {
        Task<FuncResponse<UserAuthenticationTokenResponseModel>> Token(UserAuthenticationTokenRequestModel tokenRequest);

    }
}
