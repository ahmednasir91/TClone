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
            routes.MapRoute("account", "settings/account", new { controller = "Account", action = "ProfileSettings", Settings = "Account" });
            routes.MapRoute("profile", "settings/profile", new { controller = "Account", action = "ProfileSettings", Settings = "Profile" });
            routes.MapRoute("design", "settings/design", new { controller = "Account", action = "ProfileSettings", Settings = "Design" });
            routes.MapRoute("password", "settings/password", new { controller = "Account", action = "ProfileSettings", Settings = "Password" });
            routes.MapRoute("404", "404", new {controller = "Home", action = "NotFound"});
            routes.MapRoute("following", "{username}/following", new { controller = "Home", action = "Following", username = UrlParameter.Optional });
            routes.MapRoute("followers", "{username}/followers", new { controller = "Home", action = "Followers", username = UrlParameter.Optional });
            routes.MapRoute("favorites", "{username}/favorites", new { controller = "Home", action = "Favorites", username = UrlParameter.Optional });
            routes.MapRoute("lists", "{username}/lists", new { controller = "Home", action = "Lists", username = UrlParameter.Optional });
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