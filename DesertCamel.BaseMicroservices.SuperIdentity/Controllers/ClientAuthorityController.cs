using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthorityService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthorityService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("client-authority")]
    public class ClientAuthorityController : ControllerBase
    {
        private readonly IClientAuthorityService _clientAuthorityService;

        public ClientAuthorityController(
            IClientAuthorityService clientAuthorityService)
        {
            _clientAuthorityService = clientAuthorityService;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<ClientAuthorityAddResponseModel>> Add(ClientAuthorityAddRequestModel createRequest)
        {
            return await _clientAuthorityService.Add(createRequest);
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<ClientAuthorityDeleteResponseModel>> Delete(ClientAuthorityDeleteRequestModel deleteRequest)
        {
            return await _clientAuthorityService.Delete(deleteRequest);
        }

        [HttpPost("get")]
        public async Task<FuncResponse<ClientAuthorityGetResponseModel>> Get(ClientAuthorityGetRequestModel getRequest)
        {
            return await _clientAuthorityService.Get(getRequest);
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<ClientAuthorityGetResponseModel>> List(ClientAuthorityListRequestModel listRequest)
        {
            return await _clientAuthorityService.List(listRequest);
        }
    }
}
