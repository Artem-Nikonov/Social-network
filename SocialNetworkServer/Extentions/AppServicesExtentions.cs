using Microsoft.AspNetCore.Authentication.Cookies;
using System.Runtime.CompilerServices;

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
                options.ExpireTimeSpan = TimeSpan.FromHours(3);
            });
            services.AddAuthorization();
            return services;
        }
    }
}
