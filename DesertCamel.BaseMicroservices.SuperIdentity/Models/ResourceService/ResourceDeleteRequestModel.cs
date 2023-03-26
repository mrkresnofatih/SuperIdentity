using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService
{
    public class ResourceDeleteRequestModel
    {
        [Required]
        public string Name { get; set; }
    }
}
