using StructureMap;
using ViewComposition.Renderers;

namespace ViewComposition.Services {
    public interface IRendererService {
        IRenderer GetRenderer(string name);
    }

    public class RendererService : IRendererService {
        public IRenderer GetRenderer(string name) {
            return (IRenderer) ObjectFactory.TryGetInstance(typeof (IRenderer), name);
        }
    }
}