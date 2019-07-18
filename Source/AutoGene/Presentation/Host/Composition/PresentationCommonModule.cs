using System;
using System.Resources;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Newtonsoft.Json;
using Presentation.Common.Models.Json;
using Presentation.Common.Models.Localization;
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

            builder.RegisterDefaultDependencies(Assemblies.All.Presentation());

            RegisterLocalizer(builder);
            RegisterJsonizer(builder);

            base.Load(builder);
        }
        
        private static void RegisterJsonizer(ContainerBuilder builder)
        {
            //builder.RegisterType<JsonDateTimeConrverter>().As<JsonConverter>().SingleInstance();
            //builder.RegisterType<JsonFloatConverter>().As<JsonConverter>().SingleInstance();

            builder.RegisterType<JsonText>().As<IJsonText>();
            builder.Register<Jsonizer>(container => container.Resolve<IJsonText>().Encode);
        }

        private static void RegisterLocalizer(ContainerBuilder builder)
        {
            builder.RegisterType<LocalizationManager>().As<ILocalizationManager>().SingleInstance();
            builder.RegisterType<ViewText>().As<IViewText>().SingleInstance();

            builder.Register<Localizer>(container => container.Resolve<IViewText>().Get);
        }
    }
}