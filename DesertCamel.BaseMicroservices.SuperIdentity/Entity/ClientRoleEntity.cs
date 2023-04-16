using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class ClientRoleEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClientName { get; set; }

        [Required]
        public string ResourceName { get; set; }

        public ClientEntity Client { get; set; }

        public RoleEntity Role { get; set; }
    }
}
