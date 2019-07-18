using Autofac;
using Data.Services;
using Data.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using Shared.Framework.Dependency;
using Shared.Framework.Modules;

namespace Presentation.Host.Composition
{
    /// <summary>
    /// Registers data and domain dependencies.
    /// </summary>
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RepositoryFactoriesBuilder>().SingleInstance();
            builder.RegisterType<CommonDbContext>().As<DbContext>().InstancePerRequest();
            builder.RegisterDefaultDependencies(Assemblies.All.Data());

            base.Load(builder);
        }
    }
}