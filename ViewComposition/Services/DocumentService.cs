using ViewComposition.Entities;

namespace ViewComposition.Services {
    public class DocumentService {
        private static DocumentService _instance;

        public static DocumentService Instance {
            get { return _instance ?? (_instance = new DocumentService()); }
        }

        private DocumentService() {
        }

        public Document GetDocument(string path) {
            return new Document {
                Title = "Demo Document"
            };
        }
    }
}