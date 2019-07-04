using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace Shared.Resources
{
    public class LocalizationManager : ILocalizationManager
    {
        private const string MissingKeyMessage = "MISSING KEY";

        private readonly IEnumerable<ResourceManager> resourceManagers = new List<ResourceManager>()
        {
            CommonResources.ResourceManager,
            AppResources.ResourceManager,
            CountryResources.ResourceManager,
            EcommerceResources.ResourceManager,
        };

        public string GetLocalizedString(string key, params object[] args)
        {
            string localizedString;

            foreach (ResourceManager resourceManager in resourceManagers)
            {
                localizedString = resourceManager.GetString(key);
                if (!string.IsNullOrEmpty(localizedString))
                {
                    if (args.Any())
                    {
                        localizedString = String.Format(localizedString, args);
                    }
                    return localizedString;
                }
            }

            localizedString = $"{MissingKeyMessage}: [{key}]";

            return localizedString;
        }
    }
}
