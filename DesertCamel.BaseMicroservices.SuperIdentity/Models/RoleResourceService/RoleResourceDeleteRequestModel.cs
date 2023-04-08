using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleResourceService
{
    public class RoleResourceDeleteRequestModel
    {
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string ResourceName { get; set; }
    }
}
