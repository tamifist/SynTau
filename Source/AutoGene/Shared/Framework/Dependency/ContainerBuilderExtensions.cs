using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;

namespace Shared.Framework.Dependency
{
    /// <summary>
    ///     Provides extension methods for <see cref="ContainerBuilder" />.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers the dependencies marked with marker interfaces 
        /// (<see cref="IDependency" />, <see cref="IScopedDependency"/>, <see cref="ISingletonDependency" /> or <see cref="IStartable"/>).
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="assemblies">The assemblies.</param>
        public static void RegisterDefaultDependencies(this ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.IsAssignableTo<IDependency>())
                    .AsImplementedInterfaces()
                    .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

                builder.RegisterAssemblyTypes(assembly)
                       .Where(t => t.IsAssignableTo<IDependency>())
                       .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues)
                       .AsSelf();

                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.IsAssignableTo<IScopedDependency>())
                    .AsImplementedInterfaces()
                    .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues)
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.IsAssignableTo<IScopedDependency>())
                    .AsSelf()
                    .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues)
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.IsAssignableTo<ISingletonDependency>())
                    .AsImplementedInterfaces()
                    .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues)
                    .SingleInstance();

                builder.RegisterAssemblyTypes(assembly)
                       .Where(t => t.IsAssignableTo<ISingletonDependency>())
                       //.As(t => ResolveBaseAbstract(t))
                       .AsSelf()
                       .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues)
                       .SingleInstance();

                builder.RegisterAssemblyTypes(assembly)
                       .Where(t => t.IsAssignableTo<IStartable>())
                       .As<IStartable>()
                       .SingleInstance();
            }
        }

        private static IEnumerable<Type> ResolveBaseAbstract(Type t)
        {
            if (t.BaseType != null && t.BaseType.IsAbstract)
            {
                yield return t.BaseType;
            }
        }
    }
}