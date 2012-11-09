using System.Web.Routing;
using NUnit.Framework;
using ViewComposition.Controllers;
using ViewComposition.Tests.MvcContrib;

namespace ViewComposition.Tests {
    [TestFixture]
    public class RouteTests {
        [TestFixtureSetUp]
        public void Setup() {
            var routes = RouteTable.Routes;
            routes.Clear();
            RouteConfig.RegisterRoutes(routes);
        }

        [Test]
        public void HomeIndex() {
            "~/home".ShouldMapTo<PageController>(action => action.Index("home"));
        }
    }
}