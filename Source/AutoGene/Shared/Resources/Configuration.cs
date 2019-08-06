//#define Production
//#define Staging
#define Development

using System.Collections.Specialized;

namespace Shared.Resources
{
    public class Configuration
    {
#if Production
        public static class Environment
        {
            public static readonly string Name = EnvironmentName.Production;
        }
#elif Staging
        public static class Environment
        {
            public static readonly string Name = EnvironmentName.Staging;
        }
#elif Development
        public static class Environment
        {
            public static readonly string Name = EnvironmentName.Development;

            public const string AppServiceUrl = "http://autogene.net/";
            public const string MobileServiceUrl = "http://autogenemobile.azurewebsites.net/";
            public const string SynthesizerApiUrl = "http://192.168.56.1/";
            public const string DbConnection = "Data Source=DESKTOP-EP5CCT1;Initial Catalog=autogene;User ID=sa;Password=1Q2w3e!";
            //public const string DbConnection = "Data Source=DESKTOP-EP5CCT1;Initial Catalog=syntau;User ID=sa;Password=1Q2w3e!";
        }
#endif
        public const string MigrationsAssemblyName = "Data.Migrations.All";
        public const int CookieExpirationMins = 30;

        public const string SignalHubName = "SignalHub";
    }
}