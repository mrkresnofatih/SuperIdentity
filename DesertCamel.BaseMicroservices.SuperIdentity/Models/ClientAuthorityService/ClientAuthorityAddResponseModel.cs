namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthorityService
{
    public class ClientAuthorityAddResponseModel
    {
        public Guid Id { get; set; }

        public string ClientName { get; set; }

        public string RoleResourceId { get; set; }
    }
}
