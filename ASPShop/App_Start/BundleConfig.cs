using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Web.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection collection)
        {
            collection.Add(new ScriptBundle("~/scripts")
                .Include("~/Scripts/modernizr-2.6.2.js")
                .Include("~/Scripts/jquery-1.10.2.js")
                .Include("~/Scripts/jquery-1.10.2.intellisense.js")
                .Include("~/Scripts/bootstrap.js")
                );
            collection.Add(new StyleBundle("~/styles")
                .Include("~/Content/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/Site.css", new CssRewriteUrlTransform())
                );
        }
    }
}