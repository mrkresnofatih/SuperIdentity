using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService
{
    public class ClientAuthAssignRequestModel
    {
        [Required]
        public string ClientName { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string ResourceName { get; set; }
    }
}
