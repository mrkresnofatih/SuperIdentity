namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthorityService
{
    public class UserAuthorityAddResponseModel
    {
        public Guid Id { get; set; }

        public string PrincipalName { get; set; }

        public string RoleResourceId { get; set; }
    }
}
