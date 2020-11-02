using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using VueCliMiddleware;

namespace Vue3DonetCore3SPATemplate
{
    public class Startup
    {
        protected readonly string corsPolicy = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews();

            // NOTE: PRODUCTION Ensure this is the same path that is specified in your webpack output
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddControllers();

            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy(this.corsPolicy,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    //.AllowCredentials()
                    );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(corsPolicy);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Static files
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // optional base path for IIS virtual app/directory
            //app.UsePathBase("/optionalpath");

            // Routing
            app.UseRouting();
            //app.UserAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });

            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json");
            var config = builder.Build();

            if (Boolean.Parse(config["IsLaunchSPAServer"]))
            {
                UseVueSPAServerWithVisualStudio(app);
            }
        }

        /// <summary>
        /// Launch SPA server with Visual Studio
        /// </summary>
        /// <param name="app"></param>
        protected void UseVueSPAServerWithVisualStudio(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                // use vuecliproxy for dev
                endpoints.MapToVueCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "ClientApp" },
                    npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                    regex: "Compiled successfully",
                    forceKill: true,
                    wsl: false // Set to true if you are using WSL on windows. For other operating systems it will be ignored
                    );
            });
        }
    }
}
