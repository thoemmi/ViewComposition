using System.Web.Mvc;
using ViewComposition.Models;
using ViewComposition.Services;

namespace ViewComposition.Controllers {
    public class PageController : Controller {
        public ActionResult Index(string path) {
            var page = DocumentService.Instance.GetDocument(path);

            var pageModel = new PageModel {
                Title = page.Title
            };
            return View(pageModel);
        }
    }
}