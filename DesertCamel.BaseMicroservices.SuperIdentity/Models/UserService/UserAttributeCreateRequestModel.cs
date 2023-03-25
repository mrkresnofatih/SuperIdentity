namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService
{
    public class UserAttributeCreateRequestModel
    {
        public Guid UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
