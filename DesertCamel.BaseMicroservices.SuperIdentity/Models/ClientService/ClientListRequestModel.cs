namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientListRequestModel
    {
        public string Name { get; set; }
        public long Page { get; set; }
        public long PageSize { get; set; }
    }
}
