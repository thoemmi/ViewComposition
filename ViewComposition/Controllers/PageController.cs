using System.Web.Mvc;
using ViewComposition.Models;
using ViewComposition.Services;

namespace ViewComposition.Controllers {
    public class PageController : Controller {
        public ActionResult Index(string path) {
            var document = DocumentService.Instance.GetDocument(path);

            var layout = PageLayoutService.Instance.GetLayout(document);

            var pageModel = new PageModel {
                Title = document.Title
            };
            return View(pageModel);
        }
    }
}