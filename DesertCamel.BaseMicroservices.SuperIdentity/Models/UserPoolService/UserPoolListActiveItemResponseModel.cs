namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserPoolService
{
    public class UserPoolListActiveItemResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string LoginPageUrl { get; set; }
    }
}
