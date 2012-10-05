using ViewComposition.Entities;

namespace ViewComposition.Services {
    public interface IDocumentService {
        Document GetDocument(string path);
    }

    public class DocumentService : IDocumentService {
        public Document GetDocument(string path) {
            return new Document {
                Title = "Demo Document",
                Body = "Hello world"
            };
        }
    }
}