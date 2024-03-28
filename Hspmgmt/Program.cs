using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hspmgmt.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hspmgmt;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Hspmgmt
{
    public class Startup
    {
        //constructor
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
       

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Views/Home/{0}.cshtml"); // Custom view location for Home controller
                options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml"); // Default shared views location
            });

            services.AddControllersWithViews();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //middleware
            app.UseAuthentication();

            // Use authorization

            app.UseAuthorization();

            // Inside the Configure method of Startup.cs

            // Inside the Configure method of Startup.cs

            app.UseEndpoints(endpoints =>
            {
                // Map the Index action of the PatientController
                endpoints.MapControllerRoute(
                    name: "patientIndex",
                    pattern: "patient",
                    defaults: new { controller = "Patient", action = "Index" });

                // Map the Create action of the PatientController
                endpoints.MapControllerRoute(
                    name: "patientCreate",
                    pattern: "patient/create",
                    defaults: new { controller = "Patient", action = "Create" });

                // Map the Delete action of the PatientController
                endpoints.MapControllerRoute(
                    name: "patientDelete",
                    pattern: "patient/delete/{id?}",
                    defaults: new { controller = "Patient", action = "Delete" });

                // Map the Update action of the HomeController (if required)
                endpoints.MapControllerRoute(
                    name: "patientEdit",
                    pattern: "patient/edit/{id?}",
                    defaults: new { controller = "Patient", action = "Edit" });

                // Map the default route for PatientController
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Patient}/{action=Index}/{id?}");
            });

        }
    }
}
