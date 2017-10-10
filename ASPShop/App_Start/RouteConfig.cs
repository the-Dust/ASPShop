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
                defaults: new { controller = "Product", action = "GetCatalogue", productTypeId = 0, page=1 }
            );


            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Product", action = "GetCatalogue", productTypeId = 0 },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(
                name: null,
                url: "{productTypeId}",
                defaults: new { controller = "Product", action = "GetCatalogue", page = 1 }
            );

            routes.MapRoute(
                name: null,
                url: "{productTypeId}/Page{page}",
                defaults: new { controller = "Product", action = "GetCatalogue"},
                constraints: new { page = @"\d+" }
            );

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
