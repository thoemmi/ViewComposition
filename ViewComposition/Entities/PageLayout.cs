using System.Collections.Generic;

namespace ViewComposition.Entities {
    public class PageLayout {
        private readonly List<PageSection> _sections = new List<PageSection>();

        public string Name { get; set; }

        public List<PageSection> Sections {
            get { return _sections; }
        }
    }

    public class PageSection {
        private readonly List<PagePart> _parts = new List<PagePart>();

        public string Name { get; set; }

        public List<PagePart> Parts {
            get { return _parts; }
        }
    }

    public class PagePart {
        public string Name { get; set; }
        public string Renderer { get; set; }
    }
}