using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleService
{
    public class RoleGetRequestModel
    {
        [Required]
        public string Name { get; set; }
    }
}
