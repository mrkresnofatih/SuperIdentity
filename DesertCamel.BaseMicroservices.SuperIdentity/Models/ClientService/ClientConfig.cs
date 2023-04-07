namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientConfig
    {
        public const string ClientConfigSection = "ClientJwtSettings";

        public string Issuer { get; set; }

        public string Secret { get; set; }
    }
}
