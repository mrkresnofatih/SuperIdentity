using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<UserGetResponseModel>> List([FromBody] UserListRequestModel listRequest)
        {
            var listResult = _userService.List(listRequest);
            if (listResult.IsError())
            {
                _logger.LogError(listResult.ErrorMessage);
                return new FuncListResponse<UserGetResponseModel>
                {
                    ErrorMessage = "list users failed"
                };
            }
            return listResult;
        }

        [HttpPost("get")]
        public async Task<FuncResponse<UserGetResponseModel>> Get([FromBody] UserGetRequestModel getRequest)
        {
            var getResult = _userService.Get(getRequest);
            if (getResult.IsError())
            {
                _logger.LogError(getResult.ErrorMessage);
                return new FuncResponse<UserGetResponseModel>
                {
                    ErrorMessage = "get user failed"
                };
            }
            return getResult;
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<UserDeleteResponseModel>> Delete([FromBody] UserDeleteRequestModel deleteRequest)
        {
            var deleteResult = _userService.Delete(deleteRequest);
            if (deleteResult.IsError())
            {
                _logger.LogError(deleteResult.ErrorMessage);
                return new FuncResponse<UserDeleteResponseModel>
                {
                    ErrorMessage = "delete user failed"
                };
            }
            return deleteResult;
        }
    }
}
