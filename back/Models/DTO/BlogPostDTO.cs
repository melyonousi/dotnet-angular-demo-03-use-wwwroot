﻿namespace back.Models.DTO
{
    public class BlogPostDTO
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public required string FeaturedImageUrl { get; set; }
        public string? UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public required string Author { get; set; }
        public bool IsVisible { get; set; }
    }
}
