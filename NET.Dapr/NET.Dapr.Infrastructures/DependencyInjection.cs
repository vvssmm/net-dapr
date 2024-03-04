using Microsoft.Extensions.DependencyInjection;
using NET.Dapr.Domains.Infra;

namespace NET.Dapr.Infrastructures
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<PgDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
