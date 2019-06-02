using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using Business.Contracts.ViewModels.CycleEditor;
using Business.Contracts.ViewModels.GeneEditor;
using Data.Contracts.Entities.CycleEditor;
using Microsoft.Data.Edm;
using Data.Contracts.Entities.GeneEditor;
using Data.Contracts.Entities.Identity;

namespace AutoGene.Presentation.Host
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.AddODataQueryFilter();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Register routes to OData controllers
            //modelBuilder.EntitySet<ENTITY>("CONTROLLER"); or modelBuilder.EntitySet<Model>("CONTROLLER");

            var modelBuilder = new ODataConventionModelBuilder();

            modelBuilder.EntitySet<User>("Users"); // expand Gene.User
            modelBuilder.EntitySet<Organism>("Organisms"); // expand Gene.Organism
            modelBuilder.EntitySet<GeneFragment>("GeneFragments"); // expand Gene.GeneFragments
            modelBuilder.EntitySet<Gene>("Gene");

            modelBuilder.EntitySet<HardwareFunction>("CycleStepFunction");//.HasOptionalBinding(x => x.CycleSteps, "CycleSteps");
            modelBuilder.EntitySet<HardwareFunction>("ValveFunction");//.HasOptionalBinding(x => x.CycleSteps, "CycleSteps");
            modelBuilder.EntitySet<HardwareFunction>("ActivateChannelFunction");//.HasOptionalBinding(x => x.CycleSteps, "CycleSteps");

            var oligoSynthesisCycles = modelBuilder.EntitySet<SynthesisCycle>("OligoSynthesisCycle");
            oligoSynthesisCycles.HasOptionalBinding(x => x.CycleSteps, "CycleSteps");
            oligoSynthesisCycles.HasOptionalBinding(x => x.User, "Users");

            config.Routes.MapODataServiceRoute("odata", "odata", modelBuilder.GetEdmModel());

            config.EnableSystemDiagnosticsTracing();
        }
    }
}