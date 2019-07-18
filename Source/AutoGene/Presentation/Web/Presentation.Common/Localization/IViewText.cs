namespace Presentation.Common.Localization
{
    public interface IViewText
    {
        string Get(string key, params object[] args);
    }
}
