namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService
{
    public class UserCreateRequestModel
    {
        public string PrincipalName { get; set; }

        public Guid UserPoolId { get; set; }
    }
}
