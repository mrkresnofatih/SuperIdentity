using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class UserRoleEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string UserPrincipalName { get; set; }

        [Required]
        public string ResourceName { get; set; }

        public UserEntity User { get; set; }

        public RoleEntity Role { get; set; }
    }
}
