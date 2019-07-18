using Shared.Resources;

namespace Presentation.Common.Localization
{
    public class ViewText: IViewText
    {
        private readonly ILocalizationManager localizationManager;

        public ViewText(ILocalizationManager localizationManager)
        {
            this.localizationManager = localizationManager;
        }

        public string Get(string key, params object[] args)
        {
            string localizedString = localizationManager.GetLocalizedString(key);
            return localizedString;
            //return new HtmlString(localizedString);
        }
    }
}
