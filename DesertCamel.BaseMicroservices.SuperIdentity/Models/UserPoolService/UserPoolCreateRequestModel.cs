using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserPoolService
{
    public class UserPoolCreateRequestModel
    {
        [Required]
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
        [MaxLength(300)]
        public string LoginPageUrl { get; set; }

        [Required]
        [MaxLength(300)]
        public string ExchangeTokenUrl { get; set; }

        [Required]
        [MaxLength(300)]
        public string UserInfoUrl { get; set; }

        [Required]
        [MaxLength(300)]
        public string IssuerUrl { get; set; }

        [Required]
        [MaxLength(300)]
        public string JwksUrl { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public bool UseCache { get; set; }

        [Required]
        [Range(1, 44640)]
        public long TokenLifeTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string PrincipalNameKey { get; set; }
    }
}
