using System.Collections.Generic;
using System.Web.Routing;
using NUnit.Framework;
using ViewComposition.Controllers;
using ViewComposition.Entities;
using ViewComposition.Tests.MvcContrib;

namespace ViewComposition.Tests {
    [TestFixture]
    public class RouteTests {
        private readonly IDocument _doc = new Document { Path = "home" };

        [TestFixtureSetUp]
        public void Setup() {
            var routes = RouteTable.Routes;
            routes.Clear();
            RouteConfig.RegisterRoutes(routes, path => _doc, doc => new[] {
                new RouteConfig.DocumentRoutingInfo { Controller = "Page", Action = "Archive", Pattern = "archive/{year}/{month}" },
                new RouteConfig.DocumentRoutingInfo { Controller = "Page", Action = "Archive", Pattern = "archive/{year}" },
                new RouteConfig.DocumentRoutingInfo { Controller = "Page", Action = "Archive", Pattern = "archive" },
                new RouteConfig.DocumentRoutingInfo { Controller = "Page", Action = "Index" }
            });
        }

        [Test]
        public void HomeIndex() {
            "~/home".ShouldMapTo<PageController>(action => action.Index(_doc));
        }

        [Test]
        public void ArchiveIndex() {
            "~/home/archive".ShouldMapTo<PageController>(action => action.Archive(_doc, null, null));
        }

        [Test]
        public void ArchiveWithYearIndex() {
            "~/home/archive/2012".ShouldMapTo<PageController>(action => action.Archive(_doc, 2012, null));
        }

        [Test]
        public void ArchiveWithYearAndMonthIndex() {
            "~/home/archive/2012/11".ShouldMapTo<PageController>(action => action.Archive(_doc, 2012, 11));
        }
    }
}