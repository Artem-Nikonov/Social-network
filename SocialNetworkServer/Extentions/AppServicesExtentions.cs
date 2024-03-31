using Microsoft.AspNetCore.Authentication.Cookies;
using System.Runtime.CompilerServices;
using SocialNetworkServer.Services;
using SocialNetworkServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.SocNetworkDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using SocialNetworkServer.OptionModels;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace SocialNetworkServer.Extentions
{
    public static class AppServicesExtentions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
            {
                if (expires != null)
                {
                    return expires > DateTime.UtcNow;
                }
                return false;
            }
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                //options.RequireHttpsMetadata = true;
                //options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = JWTOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = JWTOptions.AUDIENCE,
                    LifetimeValidator = LifetimeValidator,
                    ValidateLifetime = true,
                    IssuerSigningKey = JWTOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJWTProvider, JWTPrpvider>();
            services.AddScoped<RegistrationService>();
            services.AddScoped<AuthenticationService>();
            services.AddTransient<UserService>();
            services.AddScoped<UserPostsService>();
            return services;
        }

        public static IServiceCollection AddMySqlDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SocialNetworkDBContext>(options =>
            {
                options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 34)));
            });
            return services;
        }
    }
}
