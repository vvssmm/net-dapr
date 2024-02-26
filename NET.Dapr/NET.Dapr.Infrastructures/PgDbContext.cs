using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NET.Dapr.Infrastructures
{
    public class PgDbContext(IConfiguration configuration) :DbContext
    {
        protected readonly IConfiguration Configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"));
        }

    }
}
