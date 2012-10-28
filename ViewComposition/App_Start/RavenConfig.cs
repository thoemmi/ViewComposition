using System;
using System.Linq;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Client.MvcIntegration;
using StructureMap;
using ViewComposition.Entities;
using ViewComposition.Infrastructure;

namespace ViewComposition.App_Start {
    public static class RavenConfig {
        private static IDocumentStore _documentStore;

        public static void Initialize(IContainer container) {
            if (_documentStore != null) {
                return;
            }

            var documentStore = new EmbeddableDocumentStore {
                //DataDirectory = "App_Data",
                RunInMemory = true,
                UseEmbeddedHttpServer = true
            };
            documentStore.Configuration.PluginsDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, "Plugins");
            documentStore.RegisterListener(new DocumentStoreListener());
            documentStore.Initialize();

            _documentStore = documentStore;

            IndexCreation.CreateIndexes(typeof (RavenConfig).Assembly, _documentStore);

            RavenProfiler.InitializeFor(_documentStore);

            using (var session = _documentStore.OpenSession()) {
                RavenQueryStatistics stats;
                session.Query<Document>().Statistics(out stats).Take(0).ToList();
                if (stats.TotalResults == 0) {
                    // we need to create some documents
                    var rootDoc = new Document {
                        Slug = string.Empty, Title = "Home page", Body = "<p>Welcome to this site. Go and see <a href=\"/blog\">the blog</a>.</p><p><a href=\"/about\">here</a> is the about page.</p>"
                    };
                    session.Store(rootDoc);
                    session.Store(new Document {
                        ParentId = rootDoc.Id,
                        Slug = "about",
                        Title = "About",
                        Body = "This is about this site."
                    });

                    var blogDoc = new Document {
                        ParentId = rootDoc.Id,
                        Slug = "blog",
                        Title = "Blog",
                        Body = "This is my blog."
                    };
                    session.Store(blogDoc);
                    session.Store(new Document {
                        ParentId = blogDoc.Id,
                        Slug = "First",
                        Title = "my first blog post",
                        Body = "Hooray"
                    });

                    session.SaveChanges();
                }
            }

            container.Configure(x => x.For<IDocumentSession>().HybridHttpOrThreadLocalScoped().Use(() => _documentStore.OpenSession()));
        }
    }
}