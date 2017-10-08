using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Infrastructure;
using System.Data.Entity;
using DataAccess.Context;
using Web.App_Start;
using System.Web.Optimization;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //BundleTable.EnableOptimizations = true;

            Database.SetInitializer(new EfDbInitializer());

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }
    }
}
