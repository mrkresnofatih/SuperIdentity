using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthorityService
{
    public class UserAuthorityGetRequestModel
    {
        [Required]
        public string PrincipalName { get; set; }

        [Required]
        public string RoleResourceId { get; set; }
    }
}
