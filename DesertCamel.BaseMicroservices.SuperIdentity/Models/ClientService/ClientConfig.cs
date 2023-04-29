namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientConfig
    {
        public const string ClientConfigSection = "ClientConfigurationSettings";

        public string Issuer { get; set; }

        public string Code { get; set; }
    }
}
