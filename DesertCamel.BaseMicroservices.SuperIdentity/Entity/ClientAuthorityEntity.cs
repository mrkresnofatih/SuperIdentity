using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Entity
{
    public class ClientAuthorityEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string ClientName {get;set;}

        [Required]
        public string RoleResourceId { get; set; }

        public ClientEntity Client { get; set; }

        public RoleResourceEntity RoleResource { get; set; }
    }
}
