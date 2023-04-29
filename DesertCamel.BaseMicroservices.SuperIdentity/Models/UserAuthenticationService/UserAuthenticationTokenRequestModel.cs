namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService
{
    public class UserAuthenticationTokenRequestModel
    {
        public string Code { get; set; }

        public Guid UserPoolId { get; set; }
    }
}
