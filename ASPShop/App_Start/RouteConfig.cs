using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: null,
                url: "",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            /*
            routes.MapRoute(
                name: "Cat",
                url: "Page{page}",
                defaults: new { controller = "Product", action = "GetCatalogue", category = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(
                name: "CatConcrete",
                url: "{category}",
                defaults: new { controller = "Product", action = "GetCatalogue", page = 1 }
            );

            routes.MapRoute(
                name: "CatConcrete2",
                url: "{category}/Page{page}",
                defaults: new { controller = "Product", action = "GetCatalogue"},
                constraints: new { page = @"\d+" }
            );
            */
            routes.MapRoute(null, "{controller}/{action}");

            /*
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );*/
        }
    }
}
