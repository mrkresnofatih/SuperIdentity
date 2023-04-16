using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientRoleService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientRoleService;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Controllers
{
    [ApiController]
    [Route("client-role")]
    public class ClientRoleController : ControllerBase
    {
        private readonly IClientRoleService _clientRoleService;
        private readonly ILogger<ClientRoleService> _logger;

        public ClientRoleController(
            IClientRoleService clientRoleService,
            ILogger<ClientRoleService> logger)
        {
            _clientRoleService = clientRoleService;
            _logger = logger;
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<ClientRoleDeleteResponseModel>> Delete(ClientRoleDeleteRequestModel model)
        {
            return await _clientRoleService.Delete(model);
        }

        [HttpPost("get")]
        public async Task<FuncResponse<ClientRoleGetResponseModel>> Get(ClientRoleGetResponseModel model)
        {
            return await _clientRoleService.Get(model);
        }

        [HttpPost("list")]
        public async Task<FuncListResponse<ClientRoleGetResponseModel>> List(ClientRoleListRequestModel model)
        {
            return await _clientRoleService.List(model);
        }
    }
}
