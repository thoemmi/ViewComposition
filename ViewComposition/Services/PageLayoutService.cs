using ViewComposition.Entities;

namespace ViewComposition.Services {
    public interface IPageLayoutService {
        PageLayout GetLayout(Document document);
    }

    public class PageLayoutService : IPageLayoutService {
        public PageLayout GetLayout(Document document) {
            var bodyPart = new PagePart { Name = "Body", Renderer = "Body" };
            var mainSection = new PageSection { Name = "Main" };
            mainSection.Parts.Add(bodyPart);

            var twitterPart = new PagePart() { Name = "Twitter", Renderer = "Twitter" };
            var sidebarSection = new PageSection { Name = "Sidebar" };
            sidebarSection.Parts.Add(twitterPart);

            var layout = new PageLayout { Name = "BaseDocumentLayout" };
            layout.Sections.Add(mainSection);
            layout.Sections.Add(sidebarSection);

            return layout;
        }
    }
}