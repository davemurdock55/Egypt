using Egypt.Data;
using Egypt.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Egypt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var secrets = new ConfigurationBuilder()
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = Environment.GetEnvironmentVariable("MyDbConnection");

            if (connectionString == null)
            {
                connectionString = Configuration.GetConnectionString("MyDbConnection");
            }

            services.AddDbContext<mummiesContext>(options =>
                options.UseNpgsql(connectionString)
                );
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultUI()
                .AddDefaultTokenProviders()
				.AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 3;
            });

            services.AddSingleton<InferenceSession>(
              new InferenceSession("Models/model.onnx")
            );

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
				//endpoints.MapControllerRoute("id", "{controller}/{action}/{id?}", new { Controller = "Home", action = "MoreInfo" });
				
				endpoints.MapControllerRoute("typepage", "{controller}/{action}/{table}/page-{pageNum}", new { Controller = "Home", action = "Burial" });
                endpoints.MapControllerRoute("typepage", "{controller}/{action}/{table}", new { Controller = "Home", action = "Burial" });
                endpoints.MapControllerRoute("ellaid", "{controller}/{action}/{ellaid}", new { Controller = "Home", action = "MoreInfo" });
                //endpoints.MapControllerRoute("Paging", "Page{pageNum}", new { controller = "Home", action = "Burials" });
                endpoints.MapControllerRoute("type", "{controller}/{action}/{table}", new { Controller = "Home", action = "Burials", pageNum = 1 });
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
