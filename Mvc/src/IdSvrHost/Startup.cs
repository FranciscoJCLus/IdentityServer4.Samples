using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdSvrHost.Configuration;
using IdSvrHost.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using IdentityServer4.Core.Services.InMemory;
using IdentityServer4.Core.Services;

namespace IdSvrHost2
{
    public class Startup
    {
        private readonly IApplicationEnvironment _environment;

        public Startup(IApplicationEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var cert = new X509Certificate2(Path.Combine(_environment.ApplicationBasePath, "idsrv4test.pfx"), "idsrv3test");

            var builder = services.AddIdentityServer(options =>
            {
                options.SigningCertificate = cert;
                //options.EnableWelcomePage = false;
                //options.EventsOptions.RaiseErrorEvents = options.EventsOptions.RaiseFailureEvents = options.EventsOptions.RaiseInformationEvents = options.EventsOptions.RaiseSuccessEvents = true;
                //options.SiteName = "Visma Connect";
            });
            
            builder.AddInMemoryClients(Clients.Get());
            builder.AddInMemoryScopes(Scopes.Get());

            //  replace extension method call with direct DI injection (one List<InMemoryUser> instance per instance of IdenityServer)
            //builder.AddInMemoryUsers(Users.Get());
            //services.AddInstance<List<InMemoryUser>>(Users.Get());
            services.AddInstance<IProfileService>(new ProfileService());

            builder.AddCustomGrantValidator<CustomGrantValidator>();
            
            // for the UI
            services
                .AddMvc()
                .AddRazorOptions(razor =>
                {
                    razor.ViewLocationExpanders.Add(new IdSvrHost.UI.CustomViewLocationExpander());
                });
            services.AddTransient<IdSvrHost.UI.Login.LoginService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Verbose);
            loggerFactory.AddDebug(LogLevel.Verbose);

            app.UseDeveloperExceptionPage();
            app.UseIISPlatformHandler();

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
