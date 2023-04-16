using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientRoleService
{
    public class ClientRoleListRequestModel
    {
        public string? RoleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClientName { get; set; }

        public string? ResourceName { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
