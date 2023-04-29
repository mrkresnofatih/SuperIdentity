namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService
{
    public class UserAuthenticationTokenResponseModel
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string ApplicationCallbackUrl { get; set; }
    }
}
