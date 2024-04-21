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
using SocialNetworkServer.AuxiliaryClasses;

namespace SocialNetworkServer.Extentions
{
    public static class AppServicesExtentions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
            {
                if (expires == null) return false;
                return expires > DateTime.UtcNow;
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
                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        var token = context.Request.Cookies["a_c"];
                //        if (!string.IsNullOrEmpty(token))
                //            context.Token = token;
                //        return Task.CompletedTask;
                        
                //    }
                //};
            });
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJWTProvider, JWTPrpvider>();
            services.AddScoped<IPaginator, PaginationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddScoped<IPostsService, UserPostsService>();
            services.AddScoped<IPostsService, GroupPostsService>();
            services.AddScoped<IGroupsService, GroupsService>();
            services.AddScoped<IUserSubscriptionService, UserSubscriptionService>();
            services.AddScoped<ISubscribeChecker, UserSubscriptionService>();
            services.AddScoped<IGroupSubscriptionService, GroupSubscriptionService>();
            services.AddScoped<IChatsService, ChatsService>();
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
