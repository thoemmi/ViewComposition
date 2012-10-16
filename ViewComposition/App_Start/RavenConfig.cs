using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Client.MvcIntegration;
using StructureMap;
using ViewComposition.Entities;

namespace ViewComposition.App_Start {
    public static class RavenConfig {
        private static IDocumentStore _documentStore;

        public static void Initialize(IContainer container) {
            if (_documentStore != null) {
                return;
            }

            //_documentStore = new EmbeddableDocumentStore {
            //    DataDirectory = "App_Data",
            //    UseEmbeddedHttpServer = true
            //};
            _documentStore = new DocumentStore {
                Url = "http://localhost:8080",
            };
            _documentStore.Initialize();

            IndexCreation.CreateIndexes(typeof (RavenConfig).Assembly, _documentStore);

            RavenProfiler.InitializeFor(_documentStore);

            using (var session = _documentStore.OpenSession()) {
                RavenQueryStatistics stats;
                session.Query<Document>().Statistics(out stats).Take(0).ToList();
                if (stats.TotalResults == 0) {
                    // we need to create some documents
                    session.Store(new Document {
                        Path = string.Empty,
                        Title = "Home page",
                        Body = "<p>Welcome to this site. Go and see <a href=\"/blog\">the blog</a>.</p><p><a href=\"/about\">here</a> is the about page.</p>"
                    });
                    session.Store(new Document {
                        Path = "blog",
                        Title = "Blog",
                        Body = "This is my blog."
                    });
                    session.Store(new Document {
                        Path = "about",
                        Title = "About",
                        Body = "This is about this site."
                    });

                    session.SaveChanges();
                }
            }

            container.Configure(x => x.For<IDocumentSession>().HybridHttpOrThreadLocalScoped().Use(() => _documentStore.OpenSession()));
        }
    }
}