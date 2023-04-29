using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthorityService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserAuthorityService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("user-authority")]
    public class UserAuthorityController : ControllerBase
    {
        private readonly IUserAuthorityService _userAuthorityService;

        public UserAuthorityController(
            IUserAuthorityService userAuthorityService)
        {
            _userAuthorityService = userAuthorityService;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<UserAuthorityAddResponseModel>> Add(UserAuthorityAddRequestModel addRequest)
        {
            return await _userAuthorityService.Add(addRequest);
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<UserAuthorityDeleteResponseModel>> Delete(UserAuthorityDeleteRequestModel deleteRequest)
        {
            return await _userAuthorityService.Delete(deleteRequest);
        }

        [HttpPost("get")]
        public async Task<FuncResponse<UserAuthorityGetResponseModel>> Get(UserAuthorityGetRequestModel getRequest)
        {
            return await _userAuthorityService.Get(getRequest);
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<UserAuthorityGetResponseModel>> List(UserAuthorityListRequestModel listRequest)
        {
            return await _userAuthorityService.List(listRequest);
        }
    }
}
