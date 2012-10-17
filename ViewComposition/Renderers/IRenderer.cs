using StructureMap;
using ViewComposition.Entities;

namespace ViewComposition.Renderers {
    [PluginFamily]
    public interface IRenderer {
        RenderInfo Render(IDocument document);
    }
}