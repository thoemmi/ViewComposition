using System;
using Raven.Abstractions.Data;
using Raven.Database.Plugins;
using Raven.Json.Linq;

namespace ViewComposition.RavenDB {
    public class PathUpdateTrigger : AbstractPutTrigger {
        public override void OnPut(string key, RavenJObject document, RavenJObject metadata, TransactionInformation transactionInformation) {
            if (!metadata.ContainsKey("IsRoutable") || !metadata.Value<bool>("IsRoutable")) {
                return;
            }

            RavenJToken parentIdToken;
            RavenJToken slugToken;
            if (document.TryGetValue("ParentId", out parentIdToken) && document.TryGetValue("Slug", out slugToken)) {
                var parentId = parentIdToken.Value<string>();
                var slug = slugToken.Value<string>();

                string parentPath = null;
                if (!String.IsNullOrEmpty(parentId)) {
                    var parent = Database.Get(parentId, transactionInformation);
                    parentPath = parent.DataAsJson["Path"].Value<string>();
                }

                if (String.IsNullOrEmpty(parentPath)) {
                    document["Path"] = slug;
                } else {
                    document["Path"] = parentPath + "/" + slug;
                }
            }


            base.OnPut(key, document, metadata, transactionInformation);
        }
    }
}