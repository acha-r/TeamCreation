using Microsoft.AspNetCore.Authentication;
using WebApi.Services;

namespace WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<ITeamSelection, TeamSelection>();

        }
    }
}
