﻿using System.Collections.Generic;
using System.Web.Mvc;
using ViewComposition.Entities;
using ViewComposition.Models;
using ViewComposition.Renderers;
using ViewComposition.Services;

namespace ViewComposition.Controllers {
    public class PageController : Controller {
        private readonly IPageLayoutService _pageLayoutService;
        private readonly IRendererService _rendererService;

        public PageController(IPageLayoutService pageLayoutService, IRendererService rendererService) {
            _pageLayoutService = pageLayoutService;
            _rendererService = rendererService;
        }

        public ActionResult Index(IDocument document) {
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

        public ActionResult Archive(IDocument document, int? year = null, int? month = null) {
            return Index(document);
        }
    }
}