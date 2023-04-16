using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService
{
    public class ClientAuthLoginRequestModel
    {
        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientSecret { get; set; }
    }
}
