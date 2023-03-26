using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.PermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.PermissionService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("permission")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(
            IPermissionService permissionService,
            ILogger<PermissionController> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<PermissionCreateResponseModel>> Create(PermissionCreateRequestModel createRequest)
        {
            return await _permissionService.Create(createRequest);
        }

        [HttpPost("get")]
        public async Task<FuncResponse<PermissionGetResponseModel>> Get(PermissionGetRequestModel getRequest)
        {
            return await _permissionService.Get(getRequest);
        }

        [HttpPost("update")]
        public async Task<FuncResponse<PermissionUpdateResponseModel>> Update(PermissionUpdateRequestModel updateRequest)
        {
            return await _permissionService.Update(updateRequest);
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<PermissionDeleteResponseModel>> Delete(PermissionDeleteRequestModel deleteRequest)
        {
            return await _permissionService.Delete(deleteRequest);
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<PermissionGetResponseModel>> List(PermissionListRequestModel listRequest)
        {
            return await _permissionService.List(listRequest);
        }
    }
}
