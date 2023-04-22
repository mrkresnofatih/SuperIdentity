namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthorityService
{
    public class ClientAuthorityGetResponseModel
    {
        public Guid Id { get; set; }

        public string ClientName { get; set; }

        public string RoleResourceId { get; set; }
    }
}
