using System;
using System.Collections.Generic;

namespace ViewComposition.Models {
    public class ArchiveViewModel {
        public string BasePath { get; set; }
        public List<MonthStatistics> MonthStatistics { get; set; }
    }

    public class MonthStatistics {
        public DateTime MonthAndYear { get; set; }
        public int NumberOfDocuments { get; set; }
    }
}