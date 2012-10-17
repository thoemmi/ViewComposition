using StructureMap;
using ViewComposition.Entities;

namespace ViewComposition.Renderers {
    [Pluggable("Body")]
    public class BodyRenderer : IRenderer {
        public RenderInfo Render(IDocument document) {
            return new RenderInfo {
                PartialViewName = "Body",
                Model = document.Body
            };
        }
    }
}