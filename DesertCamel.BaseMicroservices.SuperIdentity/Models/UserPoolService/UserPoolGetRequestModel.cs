using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserPoolService
{
    public class UserPoolGetRequestModel
    {
        [Required]
        public Guid UserPoolId { get; set; }
    }
}
