using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lynwood.Models.AppSettings;
using Lynwood.Web.Core;
using Lynwood.Web.StartUp;

namespace Lynwood.Web.Api
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
            ConfigureAppSettings(services);

            DependencyInjection.ConfigureServices(services, Configuration);

            Cors.ConfigureServices(services);

            Authentication.ConfigureServices(services, Configuration);

            MVC.ConfigureServices(services);

            SPA.ConfigureServices(services);
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<SecurityConfig>(Configuration.GetSection("SecurityConfig"));
            services.Configure<JsonWebTokenConfig>(Configuration.GetSection("JsonWebTokenConfig"));
            services.Configure<AppKeys>(Configuration.GetSection("AppKeys"));
            services.Configure<AWSCredential>(Configuration.GetSection("AWSCredentials"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Authentication.Configure(app, env);

            Cors.Configure(app, env);

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/chathub");
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            MVC.Configure(app, env);

            SPA.Configure(app, env);

            StaticFiles.Configure(app, env);
        }
    }
}