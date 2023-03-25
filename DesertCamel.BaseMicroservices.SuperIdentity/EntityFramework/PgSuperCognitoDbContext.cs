using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework
{
    public class PgSuperCognitoDbContext : SuperCognitoDbContext
    {
        public PgSuperCognitoDbContext(DbContextOptions<PgSuperCognitoDbContext> options) : base(options)
        {
        }
    }
}
