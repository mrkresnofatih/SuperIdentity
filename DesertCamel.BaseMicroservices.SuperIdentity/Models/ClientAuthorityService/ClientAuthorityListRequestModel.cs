using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientAuthorityService
{
    public class ClientAuthorityListRequestModel
    {
        [Required]
        public string ClientName { get; set; }

        [Required]
        public int Page { get; set; }

        [Required]
        public int PageSize { get; set; }
    }
}
