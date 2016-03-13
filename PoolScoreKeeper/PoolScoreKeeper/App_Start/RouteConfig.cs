using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PoolScoreKeeper
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "PlayerCompare",
                url: "Comparing/{winnerSidePlayerName}/And/{runnerUpSidePlayerName}",
                defaults: new { controller = "Home", action = "PlayerComparison", winnerSidePlayerName = UrlParameter.Optional, runnerUpSidePlayerName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
