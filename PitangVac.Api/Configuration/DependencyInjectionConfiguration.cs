using PitangVac.Api.Middleware;
using PitangVac.Repository.Interface.IRepositories;
using PitangVac.Repository.Repositories;
using PitangVac.Utilities.UserContext;

namespace PitangVac.Api.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            InjectRepositories(services);
            InjectServices(services);
            InjectMiddleware(services);
            InjectAuthorization(services);
        }

        private static void InjectMiddleware(IServiceCollection services)
        {
            services.AddTransient<ApiMiddleware>();
            services.AddTransient<UserContextMiddleware>();
        }

        private static void InjectServices(IServiceCollection services)
        {
        }

        private static void InjectRepositories(IServiceCollection services)
        {
            services.AddScoped<ISchedulingRepository, SchedulingRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
        }

        private static void InjectAuthorization(IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
            // TODO: Implementar a configuração para autenticação
        }
    }
}
