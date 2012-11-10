using System;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;
using ViewComposition.Entities;
using ViewComposition.Infrastructure;

namespace ViewComposition.Services {
    public interface IDocumentService {
        IDocument GetDocument(string path);
    }

    public class DocumentService : IDocumentService {
        private readonly IDocumentSession _documentSession;

        public DocumentService(IDocumentSession documentSession) {
            _documentSession = documentSession;
        }

        public IDocument GetDocument(string path) {
            var query = _documentSession.Query<IDocument, Documents_ByPath>();
            if (!String.IsNullOrEmpty(path)) {
                var pathParts = path.Split('/');
                for (var i = 1; i <= pathParts.Length; ++i) {
                    var shortenedPath = String.Join("/", pathParts, startIndex : 0, count : i);
                    query = query.Search(doc => doc.Path, shortenedPath, boost : i, options : SearchOptions.Or);
                }
            } else {
                query = query.Where(doc => doc.Path == String.Empty);
            }

            var document = query.Take(1).FirstOrDefault();
            return document;
        }
    }
}