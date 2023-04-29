using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class UserAuthorityEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string PrincipalName { get; set; }

        [Required]
        public string RoleResourceId { get; set; }

        public UserEntity User { get; set; }

        public RoleResourceEntity RoleResource { get; set; }
    }
}
