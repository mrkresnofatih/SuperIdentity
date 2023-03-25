using Newtonsoft.Json;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Models
{
    public class OidcUserInfoModel
    {
        [JsonProperty("sub")]
        public string? Sub { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("given_name")]
        public string? GivenName { get; set; }

        [JsonProperty("family_name")]
        public string? FamilyName { get; set; }

        [JsonProperty("middle_name")]
        public string? MiddleName { get; set; }

        [JsonProperty("nickname")]
        public string? Nickname { get; set; }

        [JsonProperty("preferred_username")]
        public string? PreferredUsername { get; set; }

        [JsonProperty("profile")]
        public string? Profile { get; set; }

        [JsonProperty("picture")]
        public string? Picture { get; set; }

        [JsonProperty("website")]
        public string? Website { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("gender")]
        public string? Gender { get; set; }

        [JsonProperty("birthdate")]
        public string? BirthDate { get; set; }

        [JsonProperty("zoneinfo")]
        public string? ZoneInfo { get; set; }

        [JsonProperty("locale")]
        public string? Locale { get; set; }

        [JsonProperty("phone_number")]
        public string? PhoneNumber { get; set; }
    }
}
