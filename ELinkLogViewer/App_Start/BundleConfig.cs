using System.Web;
using System.Web.Optimization;

namespace ELinkLogViewer
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/Bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Bundles/Bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            
            bundles.Add(new ScriptBundle("~/Bundles/Prism").Include(
                      "~/Scripts/prism.js"));
            
            bundles.Add(new ScriptBundle("~/Bundles/Site").Include(
                      "~/Scripts/Shared/_Layout.js"));
            
            bundles.Add(new ScriptBundle("~/Bundles/Home/Index").Include(
                      "~/Scripts/Home/Index.js"));
            
            bundles.Add(new ScriptBundle("~/Bundles/LogViewer/Index").Include(
                      "~/Scripts/LogViewer/Index.js"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Styles/Shared/bootstrap.css",
                      "~/Content/Styles/Shared/prism.css",
                      "~/Content/Styles/Shared/site.css",
                      "~/Content/Styles/LogViewer/Index.css"));
        }
    }
}
