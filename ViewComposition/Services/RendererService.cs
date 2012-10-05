using System;
using System.Collections.Generic;
using ViewComposition.Renderers;

namespace ViewComposition.Services {
    public class RendererService {
        private static RendererService _instance;

        public static RendererService Instance {
            get { return _instance ?? (_instance = new RendererService()); }
        }

        private readonly Dictionary<string,IRenderer> _renderers = new Dictionary<string, IRenderer>();        

        private RendererService() {
            _renderers.Add("Twitter", new TwitterRenderer());
            _renderers.Add("Body", new BodyRenderer());
        }

        public IRenderer GetRenderer(string name) {
            IRenderer renderer;
            if (!_renderers.TryGetValue(name, out renderer)) {
                throw new ArgumentException("Unknown renderer " + name);
            }
            return renderer;
        }
    }
}