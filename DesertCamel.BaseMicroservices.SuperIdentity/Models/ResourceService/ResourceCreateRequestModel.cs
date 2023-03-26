using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService
{
    public class ResourceCreateRequestModel
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
