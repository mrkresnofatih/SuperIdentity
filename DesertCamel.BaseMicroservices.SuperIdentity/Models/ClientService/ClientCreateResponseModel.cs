namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientCreateResponseModel
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
    }
}
