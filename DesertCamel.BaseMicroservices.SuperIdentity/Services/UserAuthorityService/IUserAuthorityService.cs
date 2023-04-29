using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthorityService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.UserAuthorityService
{
    public interface IUserAuthorityService
    {
        Task<FuncResponse<UserAuthorityAddResponseModel>> Add(UserAuthorityAddRequestModel addRequest);

        Task<FuncResponse<UserAuthorityGetResponseModel>> Get(UserAuthorityGetRequestModel getRequest);

        Task<FuncResponse<UserAuthorityDeleteResponseModel>> Delete(UserAuthorityDeleteRequestModel deleteRequest);

        Task<FuncListResponse<UserAuthorityGetResponseModel>> List(UserAuthorityListRequestModel listRequest);
    }
}
