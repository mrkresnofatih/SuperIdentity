namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService
{
    public class ClientAuthTokenResponseModel
    {
        public string Token { get; set; }

        public long LifeTime { get; set; }
    }
}
