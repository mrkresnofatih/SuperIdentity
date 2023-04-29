using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.OauthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Newtonsoft.Json;
using System.Net;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.OauthService
{
    public class OauthService : IOauthService
    {
        private readonly ILogger<OauthService> _logger;

        public OauthService(
            ILogger<OauthService> logger)
        {
            _logger = logger;
        }

        public async Task<FuncResponse<OauthTokenExchangeResponseModel>> ExchangeToken(OauthTokenExchangeRequestModel exchangeRequest)
        {
            try
            {
                _logger.LogInformation($"Start ExchangeToken w. data: {exchangeRequest.ToJson()}");
                var httpClient = new HttpClient();

                var formBodyMap = new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "client_id", exchangeRequest.ClientId },
                    { "client_secret", exchangeRequest.ClientSecret },
                    { "code", exchangeRequest.Code },
                    { "redirect_uri", exchangeRequest.RedirectUri }
                };
                var request = new HttpRequestMessage(HttpMethod.Post, exchangeRequest.ExchangeTokenUrl)
                {
                    Content = new FormUrlEncodedContent(formBodyMap)
                };
                var result = await httpClient.SendAsync(request);
                var statusCode = result.StatusCode;
                var responseBody = await result.Content.ReadAsStringAsync();

                _logger.LogInformation($"ResponseBody: {responseBody}, Status: {(int)statusCode}");

                if (statusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Unsuccessful ExchangeToken");
                }

                var data = JsonConvert.DeserializeObject<OauthTokenExchangeResponseModel>(responseBody);

                _logger.LogInformation("Success: ExchangeToken");
                return new FuncResponse<OauthTokenExchangeResponseModel>
                {
                    Data = data
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to exchange token");
                return new FuncResponse<OauthTokenExchangeResponseModel>
                {
                    ErrorMessage = "failed to exchange token"
                };
            }
        }

        public async Task<FuncResponse<OauthUserInfoResponseModel>> UserInfo(OauthUserInfoRequestModel userInfoRequest)
        {
            try
            {
                _logger.LogInformation($"Start UserInfo w. data: {userInfoRequest.ToJson()}");
                var httpClient = new HttpClient();

                var request = new HttpRequestMessage(HttpMethod.Get, userInfoRequest.UserInfoUrl);
                request.Headers.Add("Authorization", $"Bearer {userInfoRequest.Token}");
                request.Headers.Add("Accept", "application/json");

                var response = await httpClient.SendAsync(request);
                var statusCode = response.StatusCode;
                var responseBody = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"ResponseBody: {responseBody}, Status: {(int)statusCode}");

                if (statusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Unsuccessful UserInfo");
                }

                var data = JsonConvert.DeserializeObject<OauthUserInfoResponseModel>(responseBody);

                _logger.LogInformation("success: userinfo");
                return new FuncResponse<OauthUserInfoResponseModel>
                {
                    Data = data
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed userinfo");
                return new FuncResponse<OauthUserInfoResponseModel>
                {
                    ErrorMessage = "failed to get userinfo"
                };
            }
        }
    }
}
