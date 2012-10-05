using StructureMap;
using ViewComposition.Entities;

namespace ViewComposition.Renderers {
    [PluginFamily]
    public interface IRenderer {
        RenderInfo Render(Document document);
    }

    public class RenderInfo {
        public string PartialViewName { get; set; }
        public object Model { get; set; }
    }
}