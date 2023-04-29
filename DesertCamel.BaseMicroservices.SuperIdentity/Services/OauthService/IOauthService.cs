using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.OauthService;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.OauthService
{
    public interface IOauthService
    {
        Task<FuncResponse<OauthTokenExchangeResponseModel>> ExchangeToken(OauthTokenExchangeRequestModel exchangeRequest);

        Task<FuncResponse<OauthUserInfoResponseModel>> UserInfo(OauthUserInfoRequestModel userInfoRequest);
    }
}
