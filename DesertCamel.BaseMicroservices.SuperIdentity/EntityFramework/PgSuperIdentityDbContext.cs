using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework
{
    public class PgSuperIdentityDbContext : SuperIdentityDbContext
    {
        public PgSuperIdentityDbContext(DbContextOptions<PgSuperIdentityDbContext> options) : base(options)
        {
        }
    }
}
