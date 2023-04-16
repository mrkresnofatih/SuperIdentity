using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientRoleService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientRoleService
{
    public interface IClientRoleService
    {
        Task<FuncResponse<ClientRoleCreateResponseModel>> Create(ClientRoleCreateRequestModel model);

        Task<FuncResponse<ClientRoleDeleteResponseModel>> Delete(ClientRoleDeleteRequestModel model);

        Task<FuncResponse<ClientRoleGetResponseModel>> Get(ClientRoleGetRequestModel model);

        Task<FuncListResponse<ClientRoleGetResponseModel>> List(ClientRoleListRequestModel model);
    }
}
