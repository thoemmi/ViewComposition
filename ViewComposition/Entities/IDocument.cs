namespace ViewComposition.Entities {
    public interface IDocument {
        string Title { get; set; }
        string Body { get; set; }
        string Path { get; set; }
    }
}