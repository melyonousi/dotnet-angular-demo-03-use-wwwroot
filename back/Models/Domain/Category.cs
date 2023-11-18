namespace back.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? UrlHandle { get; set; }
    }
}
