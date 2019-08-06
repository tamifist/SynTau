using System;
using Autofac;
using Presentation.Common.Json;
using Presentation.Common.Localization;
using Presentation.Common.Security;
using Shared.Framework.Dependency;
using Shared.Framework.Modules;
using Shared.Framework.Security;
using Shared.Resources;

namespace Presentation.Host.Composition
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdentityStorage>()
                   .As<IIdentityStorage>()
                   .SingleInstance()
                   .WithParameter(TypedParameter.From(TimeSpan.FromMinutes(Configuration.CookieExpirationMins)));

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