using System.Web;

namespace Shared.Framework.Localization
{
    /// <summary>
    /// Null localizer 
    /// </summary>
    public static class NullLocalizer
    {
        private static readonly Localizer instance;

        static NullLocalizer()
        {
            instance = (format, args) => new HtmlString(format);
        }

        /// <summary>
        /// The instance of <see cref="NullLocalizer"/> class.
        /// </summary>
        public static Localizer Instance
        {
            get
            {
                return instance;
            }
        }
    }
}