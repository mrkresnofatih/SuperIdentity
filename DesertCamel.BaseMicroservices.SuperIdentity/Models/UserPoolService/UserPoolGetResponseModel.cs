using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.UserPoolService
{
    public class UserPoolGetResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string LoginPageUrl { get; set; }

        public string ExchangeTokenUrl { get; set; }

        public string UserInfoUrl { get; set; }

        public string IssuerUrl { get; set; }

        public string JwksUrl { get; set; }

        public bool Enabled { get; set; }

        public bool UseCache { get; set; }

        public long TokenLifeTime { get; set; }

        public string PrincipalNameKey { get; set; }
    }
}
