namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.OauthService
{
    public class OauthTokenExchangeRequestModel
    {
        public string ExchangeTokenUrl { get; set; }
        
        public string Code { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string RedirectUri { get; set; }
    }
}
