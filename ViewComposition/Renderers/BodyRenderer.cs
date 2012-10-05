using ViewComposition.Entities;

namespace ViewComposition.Renderers {
    public class BodyRenderer : IRenderer {
        public RenderInfo Render(Document document) {
            return new RenderInfo {
                PartialViewName = "Body",
                Model = document.Body
            };
        }
    }
}