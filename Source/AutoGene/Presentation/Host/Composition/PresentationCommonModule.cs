using System;
using Autofac;
using Autofac.Core;
using Newtonsoft.Json;
using Presentation.Common.Controllers;
using Presentation.Common.Models.Json;
using Presentation.Common.Security;
using Shared.Framework.Dependency;
using Shared.Framework.Modules;
using Shared.Framework.Security;
using Shared.Resources;

namespace AutoGene.Presentation.Host.Composition
{
    public class PresentationCommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Get cookie expriration interval from config.
            builder.RegisterType<IdentityStorage>()
                   .As<IIdentityStorage>()
                   .SingleInstance()
                   .WithParameter(TypedParameter.From(TimeSpan.FromMinutes(30)));

            builder.RegisterType<ResourceContainer>().As<IResourceContainer>().SingleInstance();

            builder.RegisterDefaultDependencies(Assemblies.All.Presentation());
            
            RegisterJsonConverters(builder);

            base.Load(builder);
        }
        
        private static void RegisterJsonConverters(ContainerBuilder builder)
        {
            //builder.RegisterType<JsonDateTimeConrverter>().As<JsonConverter>().SingleInstance();
            //builder.RegisterType<JsonFloatConverter>().As<JsonConverter>().SingleInstance();

            builder.RegisterType<JsonText>().As<IJsonText>();
            builder.Register<Jsonizer>(container => container.Resolve<IJsonText>().Encode);
        }
    }
}