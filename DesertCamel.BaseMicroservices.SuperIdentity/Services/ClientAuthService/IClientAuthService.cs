using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthService
{
    public interface IClientAuthService
    {
        Task<FuncResponse<ClientAuthTokenResponseModel>> Token(ClientAuthTokenRequestModel tokenRequest);

        Task<FuncResponse<ClientAuthPermitResponseModel>> Permit(ClientAuthPermitRequestModel permitRequest);
    }
}
