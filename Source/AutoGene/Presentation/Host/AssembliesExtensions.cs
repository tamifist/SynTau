using System.Collections.Generic;
using System.Reflection;
using Business.Contracts.Services.GeneEditor;
using Data.Contracts;
using Data.Services;
using Infrastructure.Identity.Contracts.Services;
using Infrastructure.Identity.Services;
using Presentation.Common.Security;
using Services.Services.GeneEditor;
using Shared.Framework.Modules;

namespace AutoGene.Presentation.Host
{
    public static class AssembliesExtensions
    {
        /// <summary>
        /// This method collects all Infrastructure assemblies
        /// Assemblies should not duplicate.
        /// </summary>
        /// <returns>Returns an enumeration of Infrastructure assemblies</returns>
        public static IEnumerable<Assembly> Infrastructure(this IAssembliesLocator locator)
        {
            yield return typeof(IIdentityService).Assembly;
            yield return typeof(IdentityService).Assembly;
        }

        /// <summary>
        /// This method collects all Data assemblies
        /// Assemblies should not duplicate.
        /// </summary>
        /// <returns>Returns an enumeration of Data assemblies</returns>
        public static IEnumerable<Assembly> Data(this IAssembliesLocator locator)
        {
            yield return typeof(IUnitOfWork).Assembly;
            yield return typeof(CommonDbContext).Assembly;
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

        public static IEnumerable<Assembly> Business(this IAssembliesLocator locator)
        {
            yield return typeof(IGeneEditorService).Assembly;
            yield return typeof(GeneEditorService).Assembly;
        }
    }
}