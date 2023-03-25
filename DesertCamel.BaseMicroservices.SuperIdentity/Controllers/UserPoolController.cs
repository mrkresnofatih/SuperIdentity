using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserPoolService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserPoolService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("user-pool")]
    public class UserPoolController : ControllerBase
    {
        private readonly IUserPoolService _userPoolService;
        private readonly ILogger<UserPoolController> _logger;

        public UserPoolController(
            IUserPoolService userPoolService,
            ILogger<UserPoolController> logger)
        {
            this._userPoolService = userPoolService;
            this._logger = logger;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<UserPoolCreateResponseModel>> Create([FromBody] UserPoolCreateRequestModel createRequest)
        {
            var createResult = await _userPoolService.CreateUserPool(createRequest);
            if (createResult.IsError())
            {
                _logger.LogError(createResult.ErrorMessage);
                return new FuncResponse<UserPoolCreateResponseModel>
                {
                    ErrorMessage = "CreateUserPool failed"
                };
            }
            return createResult;
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<UserPoolGetResponseModel>> List([FromBody] UserPoolListRequestModel listRequest)
        {
            var listResult = await _userPoolService.ListUserPools(listRequest);
            if (listResult.IsError())
            {
                _logger.LogError(listResult.ErrorMessage);
                return new FuncListResponse<UserPoolGetResponseModel>
                {
                    ErrorMessage = "ListUserPool failed"
                };
            }
            return listResult;
        }

        [HttpPost("list-active")]
        public async Task<FuncListResponse<UserPoolListActiveItemResponseModel>> ListActive()
        {
            var listActiveResult = await _userPoolService.ListActiveUserPools(new UserPoolListActiveRequestModel());
            if (listActiveResult.IsError())
            {
                _logger.LogError(listActiveResult.ErrorMessage);
                return new FuncListResponse<UserPoolListActiveItemResponseModel>
                {
                    ErrorMessage = "ListActiveUserPool failed"
                };
            }
            return listActiveResult;
        }

        [HttpPost("update")]
        public async Task<FuncResponse<UserPoolUpdateResponseModel>> Update([FromBody] UserPoolUpdateRequestModel updateRequest)
        {
            var updateResult = await _userPoolService.UpdateUserPool(updateRequest);
            if (updateResult.IsError())
            {
                _logger.LogError(updateResult.ErrorMessage);
                return new FuncResponse<UserPoolUpdateResponseModel>
                {
                    ErrorMessage = "UpdateUserPool failed"
                };
            }
            return updateResult;
        }

        [HttpPost("get")]
        public async Task<FuncResponse<UserPoolGetResponseModel>> Get([FromBody] UserPoolGetRequestModel getRequest)
        {
            var getResult = await _userPoolService.GetUserPool(getRequest);
            if (getResult.IsError())
            {
                _logger.LogError(getResult.ErrorMessage);
                return new FuncResponse<UserPoolGetResponseModel>
                {
                    ErrorMessage = "GetUserPool failed"
                };
            }
            return getResult;
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<UserPoolDeleteResponseModel>> Delete([FromBody] UserPoolDeleteRequestModel deleteRequest)
        {
            var deleteResult = await _userPoolService.DeleteUserPool(deleteRequest);
            if (deleteResult.IsError())
            {
                _logger.LogError(deleteResult.ErrorMessage);
                return new FuncResponse<UserPoolDeleteResponseModel>
                {
                    ErrorMessage = "DeleteUserPOol failed"
                };
            }
            return deleteResult;
        }
    }
}
