using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace ViewComposition.Infrastructure {
// ReSharper disable InconsistentNaming
    public class Documents_ByPath : AbstractIndexCreationTask {
// ReSharper restore InconsistentNaming
        public override IndexDefinition CreateIndexDefinition() {
            return new IndexDefinition {
                Map = @"from doc in docs where doc[""@metadata""][""IsRoutable""] == true select new { doc.Path }"
            };
        }
    }
}