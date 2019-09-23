using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Data.Ecommerce.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Common.Security;
using Presentation.Host.Composition;

namespace Presentation.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<EcommerceDbContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt => opt.LoginPath = "/identity/account/login");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                    policy => policy.Requirements.Add(new AdminRequirement()));
            });

            // Add Autofac
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<DataModule>();
            containerBuilder.RegisterModule<BusinessModule>();
            containerBuilder.RegisterModule<PresentationModule>();

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.EnvironmentName = Shared.Resources.Configuration.Environment.Name;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            //            var cachePeriod = env.IsDevelopment() ? "600" : "604800";
            //            app.UseStaticFiles(new StaticFileOptions
            //            {
            //                OnPrepareResponse = ctx =>
            //                {
            //                    // Requires the following import:
            //                    // using Microsoft.AspNetCore.Http;
            //                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
            //                }
            //            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultArea",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
