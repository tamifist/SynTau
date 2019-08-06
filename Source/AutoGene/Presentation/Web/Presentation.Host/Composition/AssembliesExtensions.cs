using System.Collections.Generic;
using System.Reflection;
using Business.Identity.Services.Services;
using Data.Common.Services;
using Data.Ecommerce.Services;
using Presentation.Common.Controllers;
using Shared.Framework.Modules;

namespace Presentation.Host.Composition
{
    public static class AssembliesExtensions
    {
        /// <summary>
        /// Collects all Data assemblies
        /// Assemblies should not duplicate.
        /// </summary>
        /// <returns>Returns an enumeration of Data assemblies</returns>
        public static IEnumerable<Assembly> Data(this IAssembliesLocator locator)
        {
            yield return typeof(BaseDbContext).Assembly;
            yield return typeof(EcommerceDbContext).Assembly;
        }

        public static IEnumerable<Assembly> Business(this IAssembliesLocator locator)
        {
            yield return typeof(IdentityService).Assembly;
        }

        public static IEnumerable<Assembly> Presentation(this IAssembliesLocator locator)
        {
            yield return typeof(BaseController).Assembly;
        }
    }
}