using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.UserService
{
    public interface IUserService
    {
        FuncResponse<UserCreateResponseModel> Create(UserCreateRequestModel createRequest);

        FuncResponse<UserGetResponseModel> Get(UserGetRequestModel getRequest);

        FuncListResponse<UserGetResponseModel> List(UserListRequestModel listRequest);

        FuncResponse<UserDeleteResponseModel> Delete(UserDeleteRequestModel deleteRequest);

        FuncResponse<UserAttributeCreateResponseModel> CreateAttribute(UserAttributeCreateRequestModel createRequest);

        FuncResponse<UserAttributeUpdateResponseModel> UpdateAttribute(UserAttributeUpdateRequestModel updateRequest);
    }
}
