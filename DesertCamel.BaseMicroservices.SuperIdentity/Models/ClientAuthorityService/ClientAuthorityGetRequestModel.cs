using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthorityService
{
    public class ClientAuthorityGetRequestModel
    {
        [Required]
        public string ClientName { get; set; }

        [Required]
        public string RoleResourceId { get; set; }
    }
}
