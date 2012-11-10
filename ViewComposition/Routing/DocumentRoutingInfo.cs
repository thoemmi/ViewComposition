using System;
using System.Diagnostics;
using System.Reflection;
using System.Web.Routing;
using JetBrains.Annotations;

namespace ViewComposition.Routing {
    [DebuggerDisplay("Pattern = {Pattern}")]
    public class DocumentRoutingInfo {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Pattern { get; set; }
        public object Defaults { get; set; }

        private object _parsedRoute;

        // ReSharper disable PossibleNullReferenceException
        [CanBeNull]
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
                                                                          new object[] { remainingPath, new RouteValueDictionary(Defaults) });
            return values;
        }
        // ReSharper restore PossibleNullReferenceException
    }
}