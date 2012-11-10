using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ViewComposition.Entities;

namespace ViewComposition.Routing {
    public class DocumentRoute : RouteBase {
        public DocumentRoute(IRouteHandler routeHandler = null) {
            RouteHandler = routeHandler ?? new MvcRouteHandler();
        }

        public Func<string, IDocument> GetDocumentForPath;
        public Func<IDocument, IList<DocumentRoutingInfo>> GetRoutingInfosForDocument;

        private IRouteHandler RouteHandler { get; set; }

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            // Parse incoming URL (we trim off the first two chars since they're always "~/")
            var requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

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
            foreach (var value in values) {
                routeData.Values.Add(value.Key, value.Value);
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            return null;
        }
    }
}