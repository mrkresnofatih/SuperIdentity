using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RolePermissionService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.RolePermissionService
{
    public interface IRolePermissionService
    {
        Task<FuncResponse<RolePermissionCreateResponseModel>> Create(RolePermissionCreateRequestModel createRequest);

        Task<FuncResponse<RolePermissionGetResponseModel>> Get(RolePermissionGetRequestModel getRequest);

        Task<FuncResponse<RolePermissionDeleteResponseModel>> Delete(RolePermissionDeleteRequestModel deleteRequest);

        Task<FuncListResponse<RolePermissionGetResponseModel>> List(RolePermissionListRequestModel listRequest);

        Task<FuncListResponse<RolePermissionOptionsResponseModel>> Options(RolePermissionOptionsRequestModel optionsRequest);
    }
}
