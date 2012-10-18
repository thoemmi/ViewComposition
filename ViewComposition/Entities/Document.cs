namespace ViewComposition.Entities {
    public class Document : IDocument {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Slug { get; set; }
        public string Path { get; set; }
    }
}