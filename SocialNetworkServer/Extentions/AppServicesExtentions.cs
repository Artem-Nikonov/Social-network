using Microsoft.AspNetCore.Authentication.Cookies;
using System.Runtime.CompilerServices;
using SocialNetworkServer.Services;
using SocialNetworkServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.SocNetworkDBContext;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetworkServer.Extentions
{
    public static class AppServicesExtentions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "a_c";
                options.LoginPath = "/Account/Authorization";
                options.ClaimsIssuer = "SocNetw";
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
            });
            services.AddAuthorization();
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<RegistrationService>();
            services.AddScoped<AuthorizationService>();
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
