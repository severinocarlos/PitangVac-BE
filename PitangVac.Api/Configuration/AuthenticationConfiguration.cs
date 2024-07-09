using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PitangVac.Utilities.Configurations;
using System.Text;
using tusdotnet.Helpers;

namespace PitangVac.Api.Configuration
{
    public static class AuthenticationConfiguration
    {
        public static void AddAuthorizationConfiguration(this IServiceCollection services, IConfiguration configuracao)
        {
            var autenticacaoConfig = new AuthenticationConfig
            {
                Issuer = configuracao["Authorization:Issuer"],
                Audience = configuracao["Authorization:Audience"],
                SecretKey = configuracao["Authorization:SecretKey"],
                AccessTokenExpiration = int.Parse(configuracao["Authorization:AccessTokenExpiration"]),
                RefreshTokenExpiration = int.Parse(configuracao["Authorization:RefreshTokenExpiration"]),
            };


            services.AddCors(o => o.AddPolicy("CORS_POLICY", builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin()
                       .WithExposedHeaders(CorsHelper.GetExposedHeaders());
            }));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = autenticacaoConfig.Issuer,

                            ValidateAudience = true,
                            ValidAudience = autenticacaoConfig.Audience,

                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(autenticacaoConfig.SecretKey)),

                            RequireExpirationTime = true,
                            ValidateLifetime = true,

                            ClockSkew = TimeSpan.Zero
                        };
                    });
        }
    }
}
