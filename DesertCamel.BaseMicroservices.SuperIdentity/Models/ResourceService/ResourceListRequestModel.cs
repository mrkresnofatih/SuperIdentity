using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService
{
    public class ResourceListRequestModel
    {
        public string Name { get; set; }

        [Required]
        public long Page { get; set; }

        [Required]
        public long PageSize { get; set; }
    }
}
