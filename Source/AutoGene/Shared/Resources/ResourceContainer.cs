using System.Globalization;
using System.Resources;
//using Shared.Framework.Dependency;

namespace Shared.Resources
{
    public class ResourceContainer : IResourceContainer//, IScopedDependency
    {
        public const string ResourceId = "Shared.Resources.AppResources"; // The namespace and name of Resources file
        private readonly ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(IResourceContainer).Assembly);
        private readonly CultureInfo cultureInfo;

        //public ResourceContainer(ICultureInfoRetriever cultureInfoRetriever)
        //{
        //    cultureInfo = cultureInfoRetriever.GetCurrentCultureInfo();
        //}

        public string GetString(string key)
        {
            string translatedString = resourceManager.GetString(key); //, cultureInfo
            return translatedString != null ? translatedString : key;
        }
    }
}
