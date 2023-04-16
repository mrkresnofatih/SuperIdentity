using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientTokenRequestModel
    {
        [Required]
        [MaxLength(100)]
        public string ClientName { get; set; }
    }
}
