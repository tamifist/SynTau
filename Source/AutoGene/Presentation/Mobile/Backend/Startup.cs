using Microsoft.Owin;
using Mobile.Backend;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Mobile.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}