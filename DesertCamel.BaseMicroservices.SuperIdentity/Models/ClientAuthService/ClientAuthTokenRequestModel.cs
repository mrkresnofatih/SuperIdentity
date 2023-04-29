using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthService
{
    public class ClientAuthTokenRequestModel
    {
        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientSecret { get; set; }
    }
}
