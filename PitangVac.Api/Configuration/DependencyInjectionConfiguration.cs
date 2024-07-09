using PitangVac.Api.Middleware;
using PitangVac.Business.Business;
using PitangVac.Business.Interface.IBusiness;
using PitangVac.Repository;
using PitangVac.Repository.Interface;
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
            services.AddScoped<ITransactionManagement, TransactionManagement>();
            InjectAuthorization(services, configuration);
        }

        private static void InjectMiddleware(IServiceCollection services)
        {
            services.AddTransient<ApiMiddleware>();
            services.AddTransient<UserContextMiddleware>();
        }

        private static void InjectServices(IServiceCollection services)
        {
            services.AddScoped<IPatientBusiness, PatientBusiness>();
            services.AddScoped<IAuthenticationBusiness, AuthenticationBusiness>();
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
