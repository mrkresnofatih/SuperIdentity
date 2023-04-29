namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService
{
    public class UserCreateResponseModel
    {
        public Guid Id { get; set; }
        public string PrincipalName { get; set; }
        public Guid UserPoolId { get; set; }
    }
}
