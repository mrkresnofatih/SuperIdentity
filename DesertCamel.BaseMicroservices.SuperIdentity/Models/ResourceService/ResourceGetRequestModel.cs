using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService
{
    public class ResourceGetRequestModel
    {
        [Required]
        public string Name { get; set; }
    }
}
