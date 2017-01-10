using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace IQMedia.WebApplication
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new StyleBundle("~/Css/static").Include("~/Css/Fonts/stylesheet.css",
                                                                "~/Css/style_v2.css",
                                                                "~/Css/superfish.css"));

        }
    }
}