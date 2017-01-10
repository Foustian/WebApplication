using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IQMedia.WebApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("CopyrightAbuse", "copyright-abuse/", defaults: new { controller = "CopyrightAbusePolicy", action = "Index" });
            routes.MapRoute("ImpactVisualization", "impact-visualization/", defaults: new { controller = "ImpactVisualization", action = "Index" });
            routes.MapRoute("ImageRecognition", "image-recognition/", defaults: new { controller = "ImageRecognition", action = "Index" });
            routes.MapRoute("OrganicVideo", "organic-video/", defaults: new { controller = "OrganicVideo", action = "Index" });
            routes.MapRoute("MediaIntelligence", "media-intelligence/", defaults: new { controller = "MediaIntelligence", action = "Index" });
            routes.MapRoute("DarkSite", "discovery-light/", defaults: new { controller = "DarkSite", action = "Index" });            
            routes.MapRoute("ProQuest", "PQArticle", defaults: new { controller = "ProQuest", action = "Index" });
            routes.MapRoute("solutions", "solutions/", defaults: new { controller = "Results", action = "Index" });
            routes.MapRoute("about", "about/", defaults: new { controller = "AboutUs", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}