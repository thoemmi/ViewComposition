using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ViewComposition.Entities;

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

        private class DocumentRoute : RouteBase {
            public DocumentRoute(IRouteHandler routeHandler = null) {
                RouteHandler = routeHandler ?? new MvcRouteHandler();
            }

            public Func<string, IDocument> GetDocumentForPath;
            public Func<IDocument, IList<DocumentRoutingInfo>> GetRoutingInfosForDocument;

            private IRouteHandler RouteHandler { get; set; }

            public override RouteData GetRouteData(HttpContextBase httpContext) {
                // Parse incoming URL (we trim off the first two chars since they're always "~/")
                string requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

                var document = GetDocumentForPath(requestPath);
                if (document == null) {
                    return null;
                }

                var pageInfos = GetRoutingInfosForDocument(document);
                if (pageInfos == null || pageInfos.Count == 0) {
                    return null;
                }

                var remainingPath = requestPath.Substring((document.Path ?? String.Empty).Length);
                if (remainingPath.StartsWith("/")) {
                    remainingPath = remainingPath.Substring(1);
                }

                RouteValueDictionary values = null;
                DocumentRoutingInfo documentRoutingInfo = null;
                foreach (var pi in pageInfos) {
                    values = pi.Match(remainingPath);
                    if (values != null) {
                        documentRoutingInfo = pi;
                        break;
                    }
                }

                if (documentRoutingInfo == null) {
                    return null;
                }

                var routeData = new RouteData(this, RouteHandler);
                routeData.Values.Add("controller", documentRoutingInfo.Controller);
                routeData.Values.Add("action", documentRoutingInfo.Action);
                routeData.Values.Add("path", document.Path);
                routeData.Values.Add("document", document);
                if (values != null) {
                    foreach (var value in values) {
                        routeData.Values.Add(value.Key, value.Value);
                    }
                }

                return routeData;
            }

            public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
                return null;
            }
        }

        public class DocumentRoutingInfo {
            public string Controller { get; set; }
            public string Action { get; set; }
            public string Pattern { get; set; }
            public object Defaults { get; set; }

            private object _parsedRoute;

            public RouteValueDictionary Match(string remainingPath) {
                if (_parsedRoute == null) {
                    // to get the fully qualified name, we take a known public type and just replace the type
                    var rt = typeof (RouteData).AssemblyQualifiedName.Replace("RouteData", "RouteParser");
                    var routeParserType = Type.GetType(rt);
                    _parsedRoute = routeParserType.InvokeMember("Parse",
                                                                BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod,
                                                                null, null, new object[] { Pattern });
                }
                var values = (RouteValueDictionary) _parsedRoute.GetType()
                                                                .InvokeMember("Match",
                                                                              BindingFlags.Instance | BindingFlags.Public |
                                                                              BindingFlags.InvokeMethod, null, _parsedRoute,
                                                                              new object[]
                                                                              { remainingPath, new RouteValueDictionary(Defaults) });
                return values;
            }
        }
    }
}