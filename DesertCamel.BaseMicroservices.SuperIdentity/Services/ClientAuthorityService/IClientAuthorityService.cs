using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthorityService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthorityService
{
    public interface IClientAuthorityService
    {
        Task<FuncResponse<ClientAuthorityAddResponseModel>> Add(ClientAuthorityAddRequestModel createRequest);

        Task<FuncResponse<ClientAuthorityGetResponseModel>> Get(ClientAuthorityGetRequestModel getRequest);

        Task<FuncResponse<ClientAuthorityDeleteResponseModel>> Delete(ClientAuthorityDeleteRequestModel deleteRequest);

        Task<FuncListResponse<ClientAuthorityGetResponseModel>> List(ClientAuthorityListRequestModel listRequest);
    }
}
