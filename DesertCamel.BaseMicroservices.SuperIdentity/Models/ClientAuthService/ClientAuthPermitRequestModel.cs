using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService
{
    public class ClientAuthPermitRequestModel
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string ResourceName { get; set; }

        [Required]
        public string PermissionName { get; set; }
    }
}
