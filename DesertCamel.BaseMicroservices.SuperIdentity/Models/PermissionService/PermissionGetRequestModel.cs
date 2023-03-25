using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.PermissionService
{
    public class PermissionGetRequestModel
    {
        [Required]
        public string Name { get; set; }
    }
}
