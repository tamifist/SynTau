using Autofac;
using Autofac.Integration.Mvc;
using Data.Services;
using Data.Services.Helpers;
using Microsoft.Azure.Mobile.Server;
using Shared.Framework.Dependency;
using Shared.Framework.Modules;

namespace AutoGene.Presentation.Host.Composition
{
    /// <summary>
    /// Registers data and domain dependencies.
    /// </summary>
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RepositoryFactoriesBuilder>().SingleInstance();
            builder.RegisterType<CommonDbContext>().As<EntityContext>().InstancePerRequest();
            builder.RegisterDefaultDependencies(Assemblies.All.Data());

            base.Load(builder);
        }
    }
}