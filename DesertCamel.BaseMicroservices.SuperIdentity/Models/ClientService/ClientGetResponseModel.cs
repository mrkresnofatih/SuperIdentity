namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientGetResponseModel
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
    }
}
