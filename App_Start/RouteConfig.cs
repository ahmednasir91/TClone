using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace TwitterClone.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("following", "following", new {controller = "Home", action = "Following"});
            routes.MapRoute("followers", "followers", new { controller = "Home", action = "Followers" });

            routes.MapRoute(name: "ProfileRoute",
                            url: "{username}",
                            defaults: new {controller = "Home", Action = "Show"});

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}