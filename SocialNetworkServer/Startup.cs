using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialNetworkServer.Extentions;
using SocialNetworkServer.SocNetworkDBContext;

namespace SocialNetworkServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //services.AddMySqlDBContext(Configuration);
            services.AddCustomServices();
            services.AddCustomAuthentication();
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SocialNetworkDBContext>(options =>
            {
                options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 34)));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
