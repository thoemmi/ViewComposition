using System;
using System.Collections.Generic;
using ViewComposition.Renderers;

namespace ViewComposition.Models {
    public class PageModel {
        private readonly Dictionary<string,List<RenderInfo>> _sections = new Dictionary<string, List<RenderInfo>>(StringComparer.OrdinalIgnoreCase);

        public string Title { get; set; }

        public Dictionary<string, List<RenderInfo>> Sections {
            get { return _sections; }
        }
    }
}