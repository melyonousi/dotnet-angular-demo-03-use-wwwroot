using System.ComponentModel.DataAnnotations;

namespace back.Models.DTO
{
    public class UpdateBlogPostRequestDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        [Required]
        public required string FeaturedImageUrl { get; set; }
        public string? UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        [Required]
        public required string Author { get; set; }
        public bool IsVisible { get; set; }
    }
}
