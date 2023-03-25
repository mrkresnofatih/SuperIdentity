namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService
{
    public class UserGetResponseModel
    {
        public Guid Id { get; set; }
        public string PrincipalName { get; set; }
        public Guid UserPoolId { get; set; }
        public Dictionary<string, UserAttributeGetResponseModel> UserAttributes { get; set; }
    }
}
