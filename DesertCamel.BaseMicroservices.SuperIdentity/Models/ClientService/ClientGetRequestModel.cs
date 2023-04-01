using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientGetRequestModel
    {
        [Required]
        public string ClientName { get; set; }
    }
}
