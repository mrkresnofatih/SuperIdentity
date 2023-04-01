using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService
{
    public class ClientDeleteRequestModel
    {
        [Required]
        public string ClientName { get; set; }
    }
}
