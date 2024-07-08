using PitangVac.Entity.Entities;
using PitangVac.Repository.Interface.IRepositories;
using TaskControl.Repository.Repositories;

namespace PitangVac.Api.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            InjectRepositories(services);
            InjectServices(services);
        }

        private static void InjectServices(IServiceCollection services)
        {
        }

        private static void InjectRepositories(IServiceCollection services)
        {
        }
    }
}
