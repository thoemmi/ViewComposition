using Raven.Client.Listeners;
using Raven.Json.Linq;
using ViewComposition.Entities;

namespace ViewComposition.Infrastructure {
    public class DocumentStoreListener : IDocumentStoreListener {
        public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original) {
            var document = entityInstance as IDocument;
            if (document == null) {
                return false;
            }
            metadata.Add("IsRoutable", true);
            return true;
        }

        public void AfterStore(string key, object entityInstance, RavenJObject metadata) {
        }
    }
}