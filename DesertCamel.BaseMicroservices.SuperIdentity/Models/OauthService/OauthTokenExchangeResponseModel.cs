using Newtonsoft.Json;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.OauthService
{
    public class OauthTokenExchangeResponseModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
