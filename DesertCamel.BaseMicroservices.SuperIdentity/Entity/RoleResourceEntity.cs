using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class RoleResourceEntity
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ResourceName { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        public ResourceEntity Resource { get; set; }

        public RoleEntity Role { get; set; }

        public List<ClientAuthorityEntity> ClientAuthorities { get; set; }
    }
}
