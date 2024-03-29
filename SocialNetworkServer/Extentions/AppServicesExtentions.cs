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

namespace SocialNetworkServer.Extentions
{
    public static class AppServicesExtentions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine("hello");
            var JWTOptions= configuration.GetSection(nameof(SocialNetworkServer.OptionModels.JWTOptions)).Get<JWTOptions>();
            Console.WriteLine(JWTOptions.SecretKey);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                //options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptions.SecretKey))
                };
            });
            services.AddAuthorization();
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<RegistrationService>();
            services.AddScoped<AuthorizationService>();
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
