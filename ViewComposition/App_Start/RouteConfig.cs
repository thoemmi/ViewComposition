using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using ViewComposition.Entities;
using ViewComposition.Routing;

namespace ViewComposition {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes, Func<string, IDocument> getDocumentForPath, Func<IDocument, IList<DocumentRoutingInfo>> getRoutingInfosForDocument) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            routes.Add(new DocumentRoute {
                GetDocumentForPath = getDocumentForPath,
                GetRoutingInfosForDocument = getRoutingInfosForDocument,
            });

            routes.MapRoute(
                name : "Default",
                url : "{*path}",
                defaults : new { controller = "Page", action = "NotFound", path = UrlParameter.Optional }
                );
        }
    }
}