using PitangVac.Api.Middleware;
using PitangVac.Repository.Interface.IRepositories;
using PitangVac.Repository.Repositories;

namespace PitangVac.Api.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            InjectRepositories(services);
            InjectServices(services);
            InjectMiddleware(services);

            // TODO: Adicionar controle de transação e context de usuário
        }

        private static void InjectMiddleware(IServiceCollection services)
        {
            services.AddTransient<ApiMiddleware>();
            //TODO: Adicionar o middleware para autenticação
        }

        private static void InjectServices(IServiceCollection services)
        {
        }

        private static void InjectRepositories(IServiceCollection services)
        {
            services.AddScoped<ISchedulingRepository, SchedulingRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
        }
    }
}
