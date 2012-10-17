using StructureMap;
using ViewComposition.Entities;

namespace ViewComposition.Renderers {
    [Pluggable("Twitter")]
    public class TwitterRenderer : IRenderer {
        public RenderInfo Render(IDocument document) {
            return new RenderInfo {
                PartialViewName = "Twitter",
                Model = "thoemmi"
            };
        }
    }
}