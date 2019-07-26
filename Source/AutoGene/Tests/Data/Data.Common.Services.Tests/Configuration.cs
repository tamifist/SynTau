//#define PROD
#define DEV
//#define LOCAL

namespace Data.Common.Services.Tests
{
    public static class Configuration
    {
#if PROD
        public static class ConnectionStrings
        {
        }
#elif DEV
        public static class ConnectionStrings
        {
            public const string DbConnection = "Data Source=DESKTOP-EP5CCT1;Initial Catalog=syntau;User ID=sa;Password=1Q2w3e!";
        }
#elif LOCAL
        public static class ConnectionStrings
        {
        }
#endif
    }
}
