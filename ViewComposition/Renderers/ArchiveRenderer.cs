using System;
using System.Collections.Generic;
using StructureMap;
using ViewComposition.Entities;
using ViewComposition.Models;

namespace ViewComposition.Renderers {
    [Pluggable("Archive")]
    public class ArchiveRenderer : IRenderer {
        public RenderInfo Render(IDocument document) {
            return new RenderInfo {
                PartialViewName = "Archive",
                Model = new ArchiveViewModel {
                    MonthStatistics = new List<MonthStatistics> {
                        new MonthStatistics { MonthAndYear = new DateTime(2011, 12, 1), NumberOfDocuments = 1 },
                        new MonthStatistics { MonthAndYear = new DateTime(2012, 1, 1), NumberOfDocuments = 7 },
                        new MonthStatistics { MonthAndYear = new DateTime(2012, 2, 1), NumberOfDocuments = 4 },
                        new MonthStatistics { MonthAndYear = new DateTime(2012, 3, 1), NumberOfDocuments = 2 },
                    }
                }
            };
        }
    }
}