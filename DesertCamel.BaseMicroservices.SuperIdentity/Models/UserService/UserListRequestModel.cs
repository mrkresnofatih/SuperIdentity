namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService
{
    public class UserListRequestModel
    {
        public string? PrincipalName { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
