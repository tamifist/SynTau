//#define LOCAL

namespace Shared.Resources
{
    /// <summary>
    /// Contains system constants.
    /// </summary>
    public class Identifiers
    {
#if LOCAL
        public static class Environment
        {
            public const string AppServiceUrl = "http://192.168.100.2/autogene";
            public const string MobileServiceUrl = "http://autogenemobile.azurewebsites.net/";
            public const string SynthesizerApiUrl = "http://93.84.120.9:8888";
        }
#else
        public static class Environment
        {
            public const string AppServiceUrl = "http://autogene.net/";
            public const string MobileServiceUrl = "http://autogenemobile.azurewebsites.net/";
            public const string SynthesizerApiUrl = "http://192.168.56.1/";
        }
#endif
        public const string SignalHubName = "SignalHub";
    }
}