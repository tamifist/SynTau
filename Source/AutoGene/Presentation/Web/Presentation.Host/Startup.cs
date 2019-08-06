﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Data.Common.Contracts;
using Data.Common.Services;
using Data.Common.Services.Helpers;
using Data.Ecommerce.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Host.Composition;
using Shared.Framework.Dependency;
using Shared.Framework.Modules;

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

//        public void ConfigureServices(IServiceCollection services)
//        {
//            EcommerceDbContextOptions ecommerceDbContextOptions = EcommerceDbContextOptionsFactory.Create();
//            services.AddSingleton(ecommerceDbContextOptions);
//            services.AddDbContext<EcommerceDbContext>();
//
//            services.AddTransient<IUnitOfWork, UnitOfWork>();
//
//            services.AddMvc();
//        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<EcommerceDbContext>();

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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

//            string userName = Configuration.GetSection("AppConfiguration")["UserName"];
//            string password = Configuration.GetSection("AppConfiguration")["Password"];
        }
    }
}
