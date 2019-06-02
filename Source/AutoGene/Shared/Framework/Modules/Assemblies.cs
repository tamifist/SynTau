namespace Shared.Framework.Modules
{
    /// <summary>
    ///     A list of assemblies related to the project
    /// </summary>
    public interface IAssembliesLocator
    {
    }

    /// <summary>
    ///     Class that provides a quick way of accessing the assemblies defined in the project.
    /// </summary>
    public class Assemblies : IAssembliesLocator
    {
        private static readonly Assemblies instance = new Assemblies();

        /// <summary>
        ///     Gets current context of accessing the assemblies.
        /// </summary>
        public static IAssembliesLocator All
        {
            get
            {
                return instance;
            }
        }
    }
}