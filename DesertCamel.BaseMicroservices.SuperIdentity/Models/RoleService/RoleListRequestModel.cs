using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.RoleService
{
    public class RoleListRequestModel
    {
        public string Name { get; set; }

        [Required]
        public long PageSize { get; set; }
        
        [Required] 
        public long Page { get; set; }
    }
}
