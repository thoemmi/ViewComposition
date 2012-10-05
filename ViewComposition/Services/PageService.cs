using ViewComposition.Entities;

namespace ViewComposition.Services {
    public class PageService {
        private static PageService _instance;

        public static PageService Instance {
            get { return _instance ?? (_instance = new PageService()); }
        }

        private PageService() {
        }

        public Page GetPage(string path) {
            return new Page {
                Title = "Demo Page"
            };
        }
    }
}