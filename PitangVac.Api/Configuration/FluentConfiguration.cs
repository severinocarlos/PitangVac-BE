using FluentValidation;
using FluentValidation.AspNetCore;
using PitangVac.Validators.Fluent;

namespace PitangVac.Api.Configuration
{
    public static class FluentConfiguration
    {
        public static void AddFluentConfiguration(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<PatientRegisterValidator>();
        }
    }
}
