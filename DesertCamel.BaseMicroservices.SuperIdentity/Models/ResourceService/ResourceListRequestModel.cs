using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService
{
    public class ResourceListRequestModel
    {
        [Required]
        public Guid RoleId { get; set; }

        public string Name { get; set; }

        public long Page { get; set; }

        public long PageSize { get; set; }
    }
}
