namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserService
{
    public class UserAttributeUpdateRequestModel
    {
        public Guid UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
