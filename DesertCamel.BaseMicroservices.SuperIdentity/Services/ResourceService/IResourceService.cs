using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ResourceService
{
    public interface IResourceService
    {

        Task<FuncResponse<ResourceCreateResponseModel>> Create(ResourceCreateRequestModel createRequest);

        Task<FuncResponse<ResourceGetResponseModel>> Get(ResourceGetRequestModel getRequest);

        Task<FuncResponse<ResourceUpdateResponseModel>> Update(ResourceUpdateRequestModel updateRequest);

        Task<FuncResponse<ResourceDeleteResponseModel>> Delete(ResourceDeleteRequestModel deleteRequest);

        Task<FuncListResponse<ResourceGetResponseModel>> List(ResourceListRequestModel listRequest);

    }
}
