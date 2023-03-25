    using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class UserPoolEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string ClientId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ClientSecret { get; set; }

        [Required]
        public string LoginPageUrl { get; set; }

        [Required]
        public string ExchangeTokenUrl { get; set; }

        [Required]
        public string UserInfoUrl { get; set; }

        [Required]
        public string IssuerUrl { get; set; }

        [Required]
        public string JwksUrl { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public bool UseCache { get; set; }

        [Required]
        public long TokenLifeTime { get; set; }

        [Required]
        public string PrincipalNameKey { get; set; }

        public List<UserPoolVectorEntity> UserPoolVectors { get; set; }

        public List<UserEntity> Users { get; set; }
    }
}
