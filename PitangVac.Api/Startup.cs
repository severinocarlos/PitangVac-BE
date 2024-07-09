using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using PitangVac.Api.Configuration;
using PitangVac.Api.Middleware;

namespace PitangVac.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDependencyInjectionConfiguration(Configuration);

            services.AddDatabaseConfiguration(Configuration);

            services.AddAuthorizationConfiguration(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.MapType(typeof(TimeSpan), () => new() { Type = "string", Example = new OpenApiString("00:00:00") });
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PitangVac - Agendamento de Vacinas",
                    Version = "v1",
                    Description = "API para controlar o agendamento de vacinas contra COVID-19",
                    Contact = new() { Name = "Severino Carlos", Url = new Uri("https://www.linkedin.com/in/severinocarlos/") },
                    License = new() { Name = "Private", Url = new Uri("https://www.linkedin.com/in/severinocarlos/") },
                    TermsOfService = new Uri("http://google.com.br")
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insira o token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new() { { new() { Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() } });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agendamento de Vacinas v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ApiMiddleware>();
            app.UseMiddleware<UserContextMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
