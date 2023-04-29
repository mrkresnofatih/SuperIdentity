namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthorityService
{
    public class UserAuthorityGetResponseModel
    {
        public Guid Id { get; set; }

        public string PrincipalName { get; set; }

        public string RoleResourceId { get; set; }
    }
}
