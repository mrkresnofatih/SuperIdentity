using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.AuthService
{
    public class AuthPermitRequestModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string PermissionName { get; set; }

        [Required]
        public string ResourceName { get; set; }
    }
}
