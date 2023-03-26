using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ResourceService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("resource")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;
        private readonly ILogger<ResourceController> _logger;

        public ResourceController(
            IResourceService resourceService,
            ILogger<ResourceController> logger)
        {
            _resourceService = resourceService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<ResourceCreateResponseModel>> Create(ResourceCreateRequestModel createRequest)
        {
            return await _resourceService.Create(createRequest);
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<ResourceDeleteResponseModel>> Delete(ResourceDeleteRequestModel deleteRequest)
        {
            return await _resourceService.Delete(deleteRequest);
        }

        [HttpPost("get")]
        public async Task<FuncResponse<ResourceGetResponseModel>> Get(ResourceGetRequestModel getRequest)
        {
            return await _resourceService.Get(getRequest);
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<ResourceGetResponseModel>> List(ResourceListRequestModel listRequest)
        {
            return await _resourceService.List(listRequest);
        }

        [HttpPost("update")]
        public async Task<FuncResponse<ResourceUpdateResponseModel>> Update(ResourceUpdateRequestModel updateRequest)
        {
            return await _resourceService.Update(updateRequest);
        }
    }
}
