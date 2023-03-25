namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService
{
    public class ResourceDeleteRequestModel
    {
        public string ResourceName { get; set; }

        public Guid RoleId { get; set; }
    }
}
