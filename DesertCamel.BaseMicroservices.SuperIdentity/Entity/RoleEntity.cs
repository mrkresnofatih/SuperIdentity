using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class RoleEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public List<RoleResourceEntity> RoleResources { get; set; }
        public List<RolePermissionEntity> RolePermissions { get; set; }
        public List<UserRoleEntity> RoleUsers { get; set; }
    }
}
