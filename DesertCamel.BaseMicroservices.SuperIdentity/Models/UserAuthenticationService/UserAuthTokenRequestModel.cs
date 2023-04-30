using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthenticationService
{
    public class UserAuthTokenRequestModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public Guid UserPoolId { get; set; }
    }
}
