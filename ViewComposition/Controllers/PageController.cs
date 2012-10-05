using System.Web.Mvc;
using ViewComposition.Models;
using ViewComposition.Services;

namespace ViewComposition.Controllers {
    public class PageController : Controller {
        private readonly DocumentService _documentService = DocumentService.Instance;
        private readonly PageLayoutService _pageLayoutService = PageLayoutService.Instance;

        public ActionResult Index(string path) {
            var document = _documentService.GetDocument(path);

            var layout = _pageLayoutService.GetLayout(document);

            var pageModel = new PageModel {
                Title = document.Title
            };
            return View(pageModel);
        }
    }
}