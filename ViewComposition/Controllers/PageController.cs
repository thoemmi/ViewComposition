using System.Web.Mvc;
using ViewComposition.Models;

namespace ViewComposition.Controllers {
    public class PageController : Controller {
        public ActionResult Index(string path) {
            var pageModel = new PageModel {
                Title = "Demo page"
            };
            return View(pageModel);
        }
    }
}