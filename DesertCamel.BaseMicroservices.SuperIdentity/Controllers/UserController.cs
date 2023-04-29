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

        [HttpPost("delete")]
        public async Task<FuncResponse<UserDeleteResponseModel>> Delete(UserDeleteRequestModel deleteRequest)
        {
            return await _userService.Delete(deleteRequest);
        }

        [HttpPost("get")]
        public async Task<FuncResponse<UserGetResponseModel>> Get(UserGetRequestModel getRequest)
        {
            return await _userService.Get(getRequest);
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<UserGetResponseModel>> List(UserListRequestModel listRequest)
        {
            return await _userService.List(listRequest);
        }
    }
}
