using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthService
{
    public interface IClientAuthService
    {
        Task<FuncResponse<ClientAuthLoginResponseModel>> Login(ClientAuthLoginRequestModel loginRequest);

        Task<FuncResponse<ClientAuthPermitResponseModel>> Permit(ClientAuthPermitRequestModel permitRequest);

        Task<FuncResponse<ClientAuthAssignResponseModel>> Assign(ClientAuthAssignRequestModel assignRequest);
    }
}
