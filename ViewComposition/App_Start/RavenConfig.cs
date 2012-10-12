using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Client.MvcIntegration;
using StructureMap;

namespace ViewComposition.App_Start {
    public static class RavenConfig {
        private static IDocumentStore _documentStore;

        public static void Initialize(IContainer container) {
            if (_documentStore != null) {
                return;
            }

            _documentStore = new EmbeddableDocumentStore {
                DataDirectory = "App_Data"
            }.Initialize();

            IndexCreation.CreateIndexes(typeof (RavenConfig).Assembly, _documentStore);

            RavenProfiler.InitializeFor(_documentStore);

            container.Configure(x => x.For<IDocumentSession>().HybridHttpOrThreadLocalScoped().Use(() => _documentStore.OpenSession()));
        }
    }
}