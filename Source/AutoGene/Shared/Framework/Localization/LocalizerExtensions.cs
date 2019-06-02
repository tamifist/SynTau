using System.Linq;
using System.Web;

namespace Shared.Framework.Localization
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public delegate IHtmlString Localizer(string key, params object[] args);

    /// <summary>
    /// Provides extension methods for localizer.
    /// </summary>
    public static class LocalizerExtensions
    {
        /// <summary>
        /// Plurals the specified T.
        /// </summary>
        /// <param name="T">The T.</param>
        /// <param name="textSingular">The text singular.</param>
        /// <param name="textPlural">The text plural.</param>
        /// <param name="count">The count.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static IHtmlString Plural(this Localizer T, string textSingular, string textPlural, int count,
                                         params object[] args)
        {
            return T(count == 1 ? textSingular : textPlural, new object[]
                {
                    count
                }.Concat(args).ToArray());
        }
    }
}