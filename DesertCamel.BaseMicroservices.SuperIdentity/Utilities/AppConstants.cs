namespace DesertCamel.BaseMicroservices.SuperIdentity.Utilities
{
    public class AppConstants
    {
        public class ConfigKeys
        {
            public const string SELECTED_DATABASE = "AppDatabases:Selected";
            public const string POSTGRES_DB_CONN_STRING = "AppDatabases:Options:PostgreSQL";
        }

        public class DatabaseTypes
        {
            public const string POSTGRES = "PostgreSQL";
        }

        public class TokenConstants
        {
            public const string TOKEN_TYPE = "tokenType";
            public const string USER_TOKEN_TYPE = "user";
            public const string CLIENT_TOKEN_TYPE = "client";
            public const string ACCESS_TOKEN = "accessToken";
            public const string USER_POOL_ID = "userPoolId";
        }
    }
}
