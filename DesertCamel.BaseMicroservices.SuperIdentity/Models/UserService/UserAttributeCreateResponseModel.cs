namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService
{
    public class UserAttributeCreateResponseModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
