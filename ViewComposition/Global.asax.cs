﻿using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StructureMap;
using ViewComposition.App_Start;
using ViewComposition.Routing;
using ViewComposition.Services;

namespace ViewComposition {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes,
                                       path => ObjectFactory.Container.GetInstance<IDocumentService>().GetDocument(path),
                                       document => new[] {
                                           new DocumentRoutingInfo { Controller = "Page", Action = "Archive", Pattern = "archive/{year}/{month}" },
                                           new DocumentRoutingInfo { Controller = "Page", Action = "Archive", Pattern = "archive/{year}" },
                                           new DocumentRoutingInfo { Controller = "Page", Action = "Archive", Pattern = "archive" },
                                           new DocumentRoutingInfo { Controller = "Page", Action = "Index" }
                                       });
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RavenConfig.Initialize(ObjectFactory.Container);
        }

        protected void Application_EndRequest(object sender, EventArgs e) {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }
    }
}