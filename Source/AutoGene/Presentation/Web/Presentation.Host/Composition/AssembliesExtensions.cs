using System.Collections.Generic;
using System.Reflection;
using Data.Common.Services;
using Data.Ecommerce.Services;
using Presentation.Common.Security;
using Shared.Framework.Modules;

namespace Presentation.Host.Composition
{
    public static class AssembliesExtensions
    {
        /// <summary>
        /// This method collects all Infrastructure assemblies
        /// Assemblies should not duplicate.
        /// </summary>
        /// <returns>Returns an enumeration of Infrastructure assemblies</returns>
//        public static IEnumerable<Assembly> Infrastructure(this IAssembliesLocator locator)
//        {
//            yield return typeof(IIdentityService).Assembly;
//            yield return typeof(IdentityService).Assembly;
//        }

        /// <summary>
        /// This method collects all Data assemblies
        /// Assemblies should not duplicate.
        /// </summary>
        /// <returns>Returns an enumeration of Data assemblies</returns>
        public static IEnumerable<Assembly> Data(this IAssembliesLocator locator)
        {
            yield return typeof(BaseDbContext).Assembly;
            yield return typeof(EcommerceDbContext).Assembly;
        }

        /// <summary>
        /// This method collects all Data assemblies
        /// Assemblies should not duplicate.
        /// </summary>
        /// <returns>Returns an enumeration of Data assemblies</returns>
        public static IEnumerable<Assembly> Presentation(this IAssembliesLocator locator)
        {
            yield return typeof(IAuthenticationManager).Assembly;
        }

//        public static IEnumerable<Assembly> Business(this IAssembliesLocator locator)
//        {
//            yield return typeof(IGeneEditorService).Assembly;
//            yield return typeof(GeneEditorService).Assembly;
//        }
    }
}