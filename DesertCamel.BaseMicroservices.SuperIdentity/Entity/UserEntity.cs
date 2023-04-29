using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string PrincipalName { get; set; }

        [Required]
        public Guid UserPoolId { get; set; }

        public UserPoolEntity UserPool { get; set; }

        public List<UserAttributeEntity> UserAttributes { get; set; }

        public List<UserAuthorityEntity> UserAuthorities { get; set; }
    }
}
