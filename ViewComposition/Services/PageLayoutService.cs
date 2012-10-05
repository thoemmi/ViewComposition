using ViewComposition.Entities;

namespace ViewComposition.Services {
    public class PageLayoutService {
        private static PageLayoutService _instance;

        public static PageLayoutService Instance {
            get { return _instance ?? (_instance = new PageLayoutService()); }
        }

        private PageLayoutService() {}

        public PageLayout GetLayout(Document document) {
            var bodyPart = new PagePart { Name = "Body" };
            var mainSection = new PageSection { Name = "Main" };
            mainSection.Parts.Add(bodyPart);

            var twitterPart = new PagePart() { Name = "Twitter" };
            var sidebarSection = new PageSection { Name = "Sidebar" };
            sidebarSection.Parts.Add(twitterPart);

            var layout = new PageLayout { Name = "BaseDocumentLayout" };
            layout.Sections.Add(mainSection);
            layout.Sections.Add(sidebarSection);

            return layout;
        }
    }
}