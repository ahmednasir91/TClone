
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TwitterClone.App_Start;

namespace TwitterClone
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //var kernel = new StandardKernel();
            //var resolver = new NinjectDependencyResolver(kernel);
            //var host = new Host(resolver);
            //host.Configuration.KeepAlive = TimeSpan.FromSeconds(30);
            //RouteTable.Routes.MapHubs(resolver);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}