using StructureMap;
using ViewComposition.Entities;

namespace ViewComposition.Renderers {
    [Pluggable("Twitter")]
    public class TwitterRenderer : IRenderer {
        public RenderInfo Render(Document document) {
            return new RenderInfo {
                PartialViewName = "Twitter",
                Model = "thoemmi"
            };
        }
    }
}