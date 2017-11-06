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
            collection.Add(new ScriptBundle("~/bundles/scripts")
                .Include("~/Scripts/modernizr-2.6.2.js")
                .Include("~/Scripts/jquery-3.2.1.js")
                .Include("~/Scripts/jquery-3.2.1.intellisense.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.js")
                .Include("~/Scripts/jquery.validate-vsdoc.js")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js")
                );

            collection.Add(new StyleBundle("~/Content/AllCss")
                .Include("~/Content/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/ErrorStyles.css", new CssRewriteUrlTransform())
                .Include("~/Content/Style.css", new CssRewriteUrlTransform())
                .Include("~/Content/StyleImg.css", new CssRewriteUrlTransform())
                );
        }
    }
}