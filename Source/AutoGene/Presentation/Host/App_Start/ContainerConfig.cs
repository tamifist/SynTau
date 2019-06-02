using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace AutoGene.Presentation.Host
{
    /// <summary>
    /// Configures AutoFac container.
    /// <remarks>
    /// This is just an entry point for container configuration.
    /// Other dependencies are loaded in separate modules (classes which derived from <see cref="Autofac.Module" />).
    /// </remarks>
    /// </summary>
    public class ContainerConfig
    {
        private static Assembly ThisAssembly
        {
            get
            {
                return typeof(ContainerConfig).Assembly;
            }
        }

        /// <summary>
        /// Creates and builds autofac container.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static ContainerBuilder RegisterContainer(ContainerBuilder containerBuilder)
        {
            ContainerBuilder builder = containerBuilder;

            builder.RegisterAssemblyModules(ThisAssembly);
            RegisterWebDependencies(builder);

            return containerBuilder;
        }

        private static void RegisterWebDependencies(ContainerBuilder builder)
        {
            builder.RegisterControllers(ThisAssembly);
            builder.RegisterApiControllers(ThisAssembly);
            builder.RegisterModelBinders(ThisAssembly);
            builder.RegisterModelBinderProvider();

            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterFilterProvider();
        }
    }
}