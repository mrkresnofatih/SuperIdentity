using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientService
{
    public interface IClientService
    {
        Task<FuncResponse<ClientCreateResponseModel>> Create(ClientCreateRequestModel createRequest);
        Task<FuncResponse<ClientGetResponseModel>> Get(ClientGetRequestModel getRequest);
        Task<FuncResponse<ClientDeleteResponseModel>> Delete(ClientDeleteRequestModel deleteRequest);
        Task<FuncListResponse<ClientGetResponseModel>> List(ClientListRequestModel listRequest);
        Task<FuncResponse<ClientRotateResponseModel>> Rotate(ClientRotateRequestModel rotateRequest);
    }
}
