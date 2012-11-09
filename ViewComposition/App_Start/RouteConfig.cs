using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ViewComposition {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            //routes.MapRoute(
            //    name: "archive",
            //    url: "{path}/archive",
            //    defaults: new { controller = "Page", action = "Archive" },
            //    constraints: new { path = new MyRouteContraint() }
            //);
            routes.Add(new MyRoute());
            routes.MapRoute(
                name: "Default",
                url: "{*path}",
                defaults: new { controller = "Page", action = "Index", path = UrlParameter.Optional }
            );
        }

        private class MyRoute : RouteBase {
            private readonly PageInfo[] _pageInfos;

            public MyRoute(IRouteHandler routeHandler = null) {
                RouteHandler = routeHandler ?? new MvcRouteHandler();

                _pageInfos = new[] {
                    new PageInfo{Path = "home", Controller = "Page", Pattern = "archive/{year}", Defaults = new {action = "Archive", year=0}},
                    //new PageInfo{Path = "home", Controller = "Page", Pattern = "{action}", Defaults = new {action = "Index"}},
                    new PageInfo{Path = "home", Controller = "Page", Pattern = null, Defaults = new {action = "Index"}},
                };
            }

            private IRouteHandler RouteHandler { get; set; }

            private IList<PageInfo> GetPageInfo(string path) {
                var matchingPageInfos = _pageInfos.Where(pi => path.StartsWith(pi.Path));
                var maxMatchingLength = matchingPageInfos.Max(pi => pi.Path.Length);
                return matchingPageInfos.Where(pi => pi.Path.Length == maxMatchingLength).ToList();
            }

            public override RouteData GetRouteData(HttpContextBase httpContext) {
                // Parse incoming URL (we trim off the first two chars since they're always "~/")
                string requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

                var pageInfos = GetPageInfo(requestPath);
                if (pageInfos == null || pageInfos.Count == 0) {
                    return null;
                }

                var remainingPath = requestPath.Substring(pageInfos[0].Path.Length);
                if (remainingPath.StartsWith("/")) {
                    remainingPath = remainingPath.Substring(1);
                }

                RouteValueDictionary values = null;
                PageInfo pageInfo = null;
                foreach (var pi in pageInfos) {
                    values = pi.Match(remainingPath);
                    if (values != null) {
                        pageInfo = pi;
                        break;
                    }
                }

                if (pageInfo == null) {
                    return null;
                }

                var routeData = new RouteData(this, RouteHandler);
                routeData.Values.Add("controller", pageInfo.Controller);
                routeData.Values.Add("path", pageInfo.Path);
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

        private class PageInfo {
            public string Path { get; set; }
            public string Controller { get; set; }
            public string Pattern { get; set; }
            public object Defaults { get; set; }

            private object _parsedRoute = null;
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