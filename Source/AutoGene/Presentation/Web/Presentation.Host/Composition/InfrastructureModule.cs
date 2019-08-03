//using System.Security.Cryptography;
//using Autofac;
//using Shared.Framework.Dependency;
//using Shared.Framework.Modules;
//
//namespace Presentation.Host.Composition
//{
//    public class InfrastructureModule : Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            builder.RegisterType<SHA512CryptoServiceProvider>().As<HashAlgorithm>();
//            builder.RegisterDefaultDependencies(Assemblies.All.Infrastructure());
//
//            base.Load(builder);
//        }
//    }
//}