using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class RolePermissionEntity
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string PermissionName { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        public PermissionEntity Permission { get; set; }

        public RoleEntity Role { get; set; }
    }
}
