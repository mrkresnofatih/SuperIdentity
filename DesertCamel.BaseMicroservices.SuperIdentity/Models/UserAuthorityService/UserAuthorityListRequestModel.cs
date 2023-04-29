using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserAuthorityService
{
    public class UserAuthorityListRequestModel
    {
        [Required]
        public string PrincipalName { get; set; }

        [Required]
        public int Page { get; set; }

        [Required]
        public int PageSize { get; set; }
    }
}
