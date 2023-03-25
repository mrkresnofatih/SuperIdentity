using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserPoolService
{
    public class UserPoolDeleteRequestModel
    {
        [Required]
        public Guid UserPoolId { get; set; }
    }
}
