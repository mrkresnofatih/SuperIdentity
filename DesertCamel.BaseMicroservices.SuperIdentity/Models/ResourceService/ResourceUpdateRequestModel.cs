using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService
{
    public class ResourceUpdateRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
