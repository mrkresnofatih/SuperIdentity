using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class UserAttributeEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public UserEntity User { get; set; }
    }
}
