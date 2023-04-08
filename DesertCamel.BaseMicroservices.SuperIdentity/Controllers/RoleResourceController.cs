using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleResourceService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RoleResourceService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("roleresource")]
    public class RoleResourceController : ControllerBase
    {
        private readonly IRoleResourceService _roleResourceService;
        private readonly ILogger<RoleResourceController> _logger;

        public RoleResourceController(
            IRoleResourceService roleResourceService,
            ILogger<RoleResourceController> logger)
        {
            _roleResourceService = roleResourceService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<RoleResourceCreateResponseModel>> Create(RoleResourceCreateRequestModel createRequest)
        {
            return await _roleResourceService.Create(createRequest);
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<RoleResourceDeleteResponseModel>> Delete(RoleResourceDeleteRequestModel deleteRequest)
        {
            return await _roleResourceService.Delete(deleteRequest);
        }

        [HttpPost("get")]
        public async Task<FuncResponse<RoleResourceGetResponseModel>> Get(RoleResourceGetRequestModel getRequest)
        {
            return await _roleResourceService.Get(getRequest);
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<RoleResourceGetResponseModel>> List(RoleResourceListRequestModel listRequest)
        {
            return await _roleResourceService.List(listRequest);
        }

        [HttpPost("options")]
        public async Task<FuncListResponse<RoleResourceOptionsResponseModel>> Options(RoleResourceOptionsRequestModel optionsRequest)
        {
            return await _roleResourceService.Options(optionsRequest);
        }
    }
}
