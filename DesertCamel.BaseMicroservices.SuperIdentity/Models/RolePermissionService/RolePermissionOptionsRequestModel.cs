using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.RolePermissionService
{
    public class RolePermissionOptionsRequestModel
    {
        [Required]
        public string RoleName { get; set; }

        public string PermissionName { get; set; }

        [Required]
        public int Page { get; set; }

        [Required]
        public int PageSize { get; set; }
    }
}
