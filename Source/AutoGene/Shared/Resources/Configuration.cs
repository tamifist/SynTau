//#define PROD
//#define DEV
#define LOCAL

using System.Collections.Specialized;

namespace Shared.Resources
{
    public class Configuration
    {
#if PROD
        public static class Environment
        {
        }
#elif DEV
        public static class Environment
        {
        }
#elif LOCAL
        public static class Environment
        {
            public const string Name = "LOCAL";
            public const string AppServiceUrl = "http://autogene.net/";
            public const string MobileServiceUrl = "http://autogenemobile.azurewebsites.net/";
            public const string SynthesizerApiUrl = "http://192.168.56.1/";
            public const string DbConnection = "Data Source=DESKTOP-EP5CCT1;Initial Catalog=autogene;User ID=sa;Password=1Q2w3e!";
            //public const string DbConnection = "Data Source=DESKTOP-EP5CCT1;Initial Catalog=syntau;User ID=sa;Password=1Q2w3e!";
        }
#endif

        public const string SignalHubName = "SignalHub"; 
        public const string MigrationsAssemblyName = "Data.Migrations.All";
        public const int CookieExpirationMins = 30;
    }
}