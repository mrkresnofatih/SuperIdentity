using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientController> _logger;

        public ClientController(
            IClientService clientService,
            ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<ClientCreateResponseModel>> Create(ClientCreateRequestModel createRequest)
        {
            return await _clientService.Create(createRequest);
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<ClientDeleteResponseModel>> Delete(ClientDeleteRequestModel deleteRequest)
        {
            return await _clientService.Delete(deleteRequest);
        }

        [HttpPost("get")]
        public async Task<FuncResponse<ClientGetResponseModel>> Get(ClientGetRequestModel getRequest)
        {
            return await _clientService.Get(getRequest);
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<ClientGetResponseModel>> List(ClientListRequestModel listRequest)
        {
            return await _clientService.List(listRequest);
        }

        [HttpPost("rotate")]
        public async Task<FuncResponse<ClientRotateResponseModel>> Rotate(ClientRotateRequestModel rotateRequest)
        {
            return await _clientService.Rotate(rotateRequest);
        }
    }
}
