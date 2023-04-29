using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.AuthService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.AuthService
{
    public interface IAuthService
    {
        Task<FuncResponse<AuthPermitResponseModel>> Permit(AuthPermitRequestModel permitRequest);
    }
}
