using Microsoft.Extensions.DependencyInjection;
using NET.Dapr.Domains.Services;

namespace NET.Dapr.Domains
{
    public static class DependencyInjection
    {
        public static void InjectService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfiles));
            services.AddScoped<ILeaveRequestService, LeaveRequestService>();
            services.AddScoped<ITaskService, TasksService>();
        }
    }
}
