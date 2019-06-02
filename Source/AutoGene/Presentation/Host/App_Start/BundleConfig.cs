using System.Web.Optimization;

namespace AutoGene.Presentation.Host
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = false;

            bundles.IgnoreList.Clear();
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*intellisense.js");

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/bower_components/metisMenu/dist/metisMenu.min.css",
                      //"~/Content/timeline.css",
                      "~/Content/sb-admin-2.css",
                      "~/Content/autogene.css"));

            bundles.Add(new ScriptBundle("~/bundles/validator").Include(
                "~/Scripts/app/validator.js"));

            bundles.Add(new ScriptBundle("~/bundles/viewmodels").IncludeDirectory(
                "~/Scripts/app/view-models/", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/app/common.js"));
        }
    }
}
