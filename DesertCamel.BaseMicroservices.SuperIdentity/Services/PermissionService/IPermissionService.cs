using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.PermissionService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.PermissionService
{
    public interface IPermissionService
    {
        Task<FuncResponse<PermissionCreateResponseModel>> Create(PermissionCreateRequestModel createRequest);

        Task<FuncResponse<PermissionGetResponseModel>> Get(PermissionGetRequestModel getRequest);

        Task<FuncResponse<PermissionUpdateResponseModel>> Update(PermissionUpdateRequestModel updateRequest);

        Task<FuncResponse<PermissionDeleteResponseModel>> Delete(PermissionDeleteRequestModel deleteRequest);

        Task<FuncListResponse<PermissionGetResponseModel>> List(PermissionListRequestModel listRequest);
    }
}
