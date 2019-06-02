using System.Web.Http;
using AutoGene.Presentation.Host;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace AutoGene.Presentation.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            //app.MapSignalR(new HubConfiguration
            //{
            //    EnableDetailedErrors = true,
            //});

            //GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;
        }
    }
}