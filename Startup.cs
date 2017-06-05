using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using JDash.NetCore.Api;
using JDash.NetCore.Models;
using Microsoft.AspNetCore.Http;
using JDash.NetCore.Provider.MsSQL;
using JDash.NetCore.Provider.MySQL;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
                builder.AllowCredentials();
            });


            app.UseJDash<JDashConfigurator>().UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
                builder.AllowCredentials();
            });

        }

        private void ServeStaticJDashPageHTML(IApplicationBuilder obj)
        {
            obj.Run((HttpContext context) =>
            {
                //context.Response.StatusCode = 200;
                //context.Response.WriteAsync()
                return null;
            });
        }

        private bool CheckRequestIsNotJDash(HttpContext context)
        {
            return !(context.Request.Path.HasValue && context.Request.Path.ToString().Contains("jdash"));
        }
    }

    public class JDashConfigurator : BaseJDashConfigurator
    {

        public JDashConfigurator(HttpContext context) : base(context)
        {
            this.EnsureTablesCreated = true;
        }


        public override JDashPrincipalResult GetJDashPrincipal(string authorizationHeader)
        {
            // this part can be used for authentication of jdash requests
            // this.HttpContext.User.Identity.Name can be used if you are using cookie authorization
            // or you can use authorization header for custom jwt authentication.
            var username = authorizationHeader.Substring("Bearer ".Length);
            return new JDashPrincipalResult() { appid = "1", user = username };
        }

        public override IJDashPersistenceProvider GetPersistanceProvider()
        {
            string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=DemoJDash;Integrated Security=SSPI;";
            var provider = new JSQLProvider(connectionString);
            return provider;
        }
    }
}
