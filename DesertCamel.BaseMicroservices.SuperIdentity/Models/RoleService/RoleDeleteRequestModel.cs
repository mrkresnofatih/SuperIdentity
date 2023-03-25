using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleService
{
    public class RoleDeleteRequestModel
    {
        [Required]
        public string Name { get; set; }
    }
}
