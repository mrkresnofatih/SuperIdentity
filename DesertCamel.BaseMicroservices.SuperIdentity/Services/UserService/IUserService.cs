using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.UserService
{
    public interface IUserService
    {
        Task<FuncResponse<UserCreateResponseModel>> Create(UserCreateRequestModel createRequest);

        Task<FuncResponse<UserGetResponseModel>> Get(UserGetRequestModel getRequest);

        Task<FuncListResponse<UserGetResponseModel>> List(UserListRequestModel listRequest);

        Task<FuncResponse<UserDeleteResponseModel>> Delete(UserDeleteRequestModel deleteRequest);

        Task<FuncResponse<UserAttributeCreateResponseModel>> CreateAttribute(UserAttributeCreateRequestModel createRequest);

        Task<FuncResponse<UserAttributeUpdateResponseModel>> UpdateAttribute(UserAttributeUpdateRequestModel updateRequest);
    }
}
