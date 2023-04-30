namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService
{
    public class UserAuthTokenResponseModel
    {
        public string Token { get; set; }

        public string ApplicationCallbackUrl { get; set; }
    }
}
