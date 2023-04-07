using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RolePermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RolePermissionService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("rolepermission")]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;
        private readonly ILogger<RolePermissionController> _logger;

        public RolePermissionController(
            IRolePermissionService rolePermissionService,
            ILogger<RolePermissionController> logger)
        {
            _rolePermissionService = rolePermissionService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<RolePermissionCreateResponseModel>> Create(RolePermissionCreateRequestModel createRequest)
        {
            return await _rolePermissionService.Create(createRequest);
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<RolePermissionDeleteResponseModel>> Delete(RolePermissionDeleteRequestModel deleteRequest)
        {
            return await _rolePermissionService.Delete(deleteRequest);
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<RolePermissionGetResponseModel>> List(RolePermissionListRequestModel listRequest)
        {
            return await _rolePermissionService.List(listRequest);
        }

        [HttpPost("options")]
        public async Task<FuncListResponse<RolePermissionOptionsResponseModel>> Options(RolePermissionOptionsRequestModel optionsRequest)
        {
            return await _rolePermissionService.Options(optionsRequest);
        }
    }
}
