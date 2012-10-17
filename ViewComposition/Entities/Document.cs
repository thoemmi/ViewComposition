namespace ViewComposition.Entities {
    public class Document : IDocument {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Path { get; set; }
    }
}