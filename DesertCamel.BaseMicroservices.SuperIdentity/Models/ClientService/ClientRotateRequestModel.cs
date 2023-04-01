using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientRotateRequestModel
    {
        [Required]
        public string ClientName { get; set; }
    }
}
