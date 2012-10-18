namespace ViewComposition.Entities {
    public interface IDocument {
        string Id { get; set; }
        string ParentId { get; set; }
        string Title { get; set; }
        string Body { get; set; }
        string Slug { get; set; }
        string Path { get; set; }
    }
}