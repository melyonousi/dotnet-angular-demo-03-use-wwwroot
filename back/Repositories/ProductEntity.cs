namespace back.Repositories
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Thumbnail { get; set; }
        public string? Banner { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool Visibility { get; set; }
    }
}
