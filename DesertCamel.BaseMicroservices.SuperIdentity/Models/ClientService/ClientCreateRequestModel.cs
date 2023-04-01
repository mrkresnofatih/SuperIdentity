using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientCreateRequestModel
    {
        [Required]
        [MaxLength(50)]
        public string ClientName { get; set; }
    }
}
