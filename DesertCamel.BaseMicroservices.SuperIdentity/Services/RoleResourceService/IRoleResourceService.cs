using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleResourceService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.RoleResourceService
{
    public interface IRoleResourceService
    {
        Task<FuncResponse<RoleResourceCreateResponseModel>> Create(RoleResourceCreateRequestModel createRequest);

        Task<FuncResponse<RoleResourceDeleteResponseModel>> Delete(RoleResourceDeleteRequestModel deleteRequest);

        Task<FuncResponse<RoleResourceGetResponseModel>> Get(RoleResourceGetRequestModel getRequest);

        Task<FuncListResponse<RoleResourceGetResponseModel>> List(RoleResourceListRequestModel listRequest);

        Task<FuncListResponse<RoleResourceOptionsResponseModel>> Options(RoleResourceOptionsRequestModel optionsRequest);
    }
}
