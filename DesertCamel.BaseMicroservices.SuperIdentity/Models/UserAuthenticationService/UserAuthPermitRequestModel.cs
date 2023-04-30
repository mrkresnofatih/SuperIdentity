using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService
{
    public class UserAuthPermitRequestModel
    {
        [Required]
        public Guid UserPoolId { get; set; }

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string PermissionName { get; set; }

        [Required]
        public string ResourceName { get; set; }
    }
}
