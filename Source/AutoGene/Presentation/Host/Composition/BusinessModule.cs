using Autofac;
using Shared.Framework.Dependency;
using Shared.Framework.Modules;

namespace AutoGene.Presentation.Host.Composition
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDefaultDependencies(Assemblies.All.Business());

            base.Load(builder);
        }
    }
}