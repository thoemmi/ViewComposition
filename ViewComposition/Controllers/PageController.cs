using System.Collections.Generic;
using System.Web.Mvc;
using ViewComposition.Models;
using ViewComposition.Renderers;
using ViewComposition.Services;

namespace ViewComposition.Controllers {
    public class PageController : Controller {
        private readonly DocumentService _documentService = DocumentService.Instance;
        private readonly PageLayoutService _pageLayoutService = PageLayoutService.Instance;
        private readonly RendererService _rendererService = RendererService.Instance;

        public ActionResult Index(string path) {
            var document = _documentService.GetDocument(path);
            var layout = _pageLayoutService.GetLayout(document);

            var pageModel = new PageModel {
                Title = document.Title
            };

            foreach (var section in layout.Sections) {
                var list = new List<RenderInfo>();

                foreach (var pagePart in section.Parts) {
                    var renderer = _rendererService.GetRenderer(pagePart.Renderer);
                    var renderInfo = renderer.Render(document);
                    list.Add(renderInfo);
                }

                pageModel.Sections.Add(section.Name, list);
            }

            return View(pageModel);
        }
    }
}