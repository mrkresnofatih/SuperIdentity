using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class UserPoolVectorEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserPoolId { get; set; }

        [Required]
        public string SourceKey { get; set; }

        [Required]
        public string DestinationKey { get; set; }

        public UserPoolEntity UserPool { get; set; }
    }
}
