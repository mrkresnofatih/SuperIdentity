using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.PermissionService
{
    public class PermissionDeleteRequestModel
    {
        [Required]
        public string Name { get; set; }
    }
}
