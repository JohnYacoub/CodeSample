using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Lynwood.Web.Api.StartUp;
using System;

namespace Lynwood.Web.Api
{
    public class Program
    {
        private static bool IsLocalDeployment = true;
        private static bool IsInProcessDeployment = true;

        public static void Main(string[] args)
        {
            if (IsLocalDeployment && IsInProcessDeployment)
            {
                CurrentDirectorySetUp.SetCurrentDirectory();

            }
            else if (IsInProcessDeployment)
            {
                CurrentDirectorySetUp.SetCurrentDirectory();
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (IsLocalDeployment && IsInProcessDeployment)
            {
                return WebHost.CreateDefaultBuilder(args)
                    .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                    //.UseKestrel()
                    .UseIISIntegration()
                    //.UseIIS()
                    .ConfigureAppConfiguration(ConfigConfiguration)
                    .ConfigureLogging(ConfigureLogging)
                    .UseStartup<Startup>();

            }
            if (IsInProcessDeployment)
            {
                return WebHost.CreateDefaultBuilder(args)
                    .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                    //.UseKestrel()
                    .UseIISIntegration()
                    //.UseIIS()
                    .ConfigureAppConfiguration(ConfigConfiguration)
                    .ConfigureLogging(ConfigureLogging)
                    .UseStartup<Startup>();
            }

            // else?

            return WebHost.CreateDefaultBuilder(args)
            .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
            //.UseKestrel()
            .UseIISIntegration()
            //.UseIIS()
            .ConfigureAppConfiguration(ConfigConfiguration)
            .ConfigureLogging(ConfigureLogging)
            .UseStartup<Startup>();
        }

        private static void ConfigureLogging(WebHostBuilderContext ctx, ILoggingBuilder logging)
        {
            logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));

            Action<ConsoleLoggerOptions> ConsoleOptions = delegate (ConsoleLoggerOptions opts)
            {
                opts.DisableColors = false;
                opts.IncludeScopes = true;
            };

            logging.AddConsole(ConsoleOptions);
            logging.AddDebug();
        }

        private static void ConfigConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder config)
        {
            IConfigurationBuilder root = config.SetBasePath(ctx.HostingEnvironment.ContentRootPath);

            //the settings in the env settings will override the appsettings.json values, recursively at the key level.
            // where the key could be nested. this would allow very fine tuned control over the settings
            IConfigurationBuilder appSettings = root.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            string jsonFileName = $"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json";
            IConfigurationBuilder envSettings = appSettings
                .AddJsonFile(jsonFileName, optional: true, reloadOnChange: true);
        }




    }
}