using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RoleService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            IRoleService roleService,
            ILogger<RoleController> logger)
        {
            this._roleService = roleService;
            this._logger = logger;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<RoleCreateResponseModel>> Create([FromBody] RoleCreateRequestModel createRequest)
        {
            var createResult = await _roleService.CreateRole(createRequest);
            if (createResult.IsError())
            {
                _logger.LogError(createResult.ErrorMessage);
                return new FuncResponse<RoleCreateResponseModel>
                {
                    ErrorMessage = "CreateRole failed"
                };
            }
            return createResult;
        }

        [HttpPost("get")]
        public async Task<FuncResponse<RoleGetResponseModel>> Get([FromBody] RoleGetRequestModel getRequest)
        {
            var getResult = await _roleService.GetRole(getRequest);
            if (getResult.IsError())
            {
                _logger.LogError(getResult.ErrorMessage);
                return new FuncResponse<RoleGetResponseModel>
                {
                    ErrorMessage = "GetRole failed"
                };
            }
            return getResult;
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<RoleGetResponseModel>> List([FromBody] RoleListRequestModel listRequest)
        {
            var listResult = await _roleService.ListRoles(listRequest);
            if (listResult.IsError())
            {
                _logger.LogError(listResult.ErrorMessage);
                return new FuncListResponse<RoleGetResponseModel>
                {
                    ErrorMessage = "ListRole failed"
                };
            }
            return listResult;
        }

        [HttpPost("update")]
        public async Task<FuncResponse<RoleUpdateResponseModel>> Update([FromBody] RoleUpdateRequestModel updateRequest)
        {
            var updateResult = await _roleService.UpdateRole(updateRequest);
            if (updateResult.IsError())
            {
                _logger.LogError(updateResult.ErrorMessage);
                return new FuncResponse<RoleUpdateResponseModel>
                {
                    ErrorMessage = "UpdateRole failed"
                };
            }
            return updateResult;
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<RoleDeleteResponseModel>> Delete([FromBody] RoleDeleteRequestModel deleteRequest)
        {
            var deleteResult = await _roleService.DeleteRole(deleteRequest);
            if (deleteResult.IsError())
            {
                _logger.LogError(deleteResult.ErrorMessage);
                return new FuncResponse<RoleDeleteResponseModel>
                {
                    ErrorMessage = "DeleteRole failed"
                };
            }
            return deleteResult;
        }
    }
}

