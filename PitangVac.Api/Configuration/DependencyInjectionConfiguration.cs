using PitangVac.Api.Middleware;
using PitangVac.Repository.Interface.IRepositories;
using PitangVac.Repository.Repositories;
using PitangVac.Utilities.Configurations;
using PitangVac.Utilities.UserContext;

namespace PitangVac.Api.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            InjectRepositories(services);
            InjectServices(services);
            InjectMiddleware(services);
            InjectAuthorization(services, configuration);

            // Todo: implementar controle de transação
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

        private static void InjectAuthorization(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserContext, UserContext>();
            services.AddOptions<AuthenticationConfig>().Bind(configuration.GetSection("Authorization"));
        }
    }
}
