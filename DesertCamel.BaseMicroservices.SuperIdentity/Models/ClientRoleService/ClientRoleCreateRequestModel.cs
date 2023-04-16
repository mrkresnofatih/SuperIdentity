using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientRoleService
{
    public class ClientRoleCreateRequestModel
    {
        [Required]
        public string RoleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClientName { get; set; }

        [Required]
        public string ResourceName { get; set; }
    }
}
