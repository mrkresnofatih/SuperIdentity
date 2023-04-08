using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleResourceService
{
    public class RoleResourceOptionsRequestModel
    {
        [Required]
        public string RoleName { get; set; }

        public string ResourceName { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int Page { get; set; }

        [Required]
        [Range(minimum: 10, maximum: 100)]
        public int PageSize { get; set; }
    }
}
