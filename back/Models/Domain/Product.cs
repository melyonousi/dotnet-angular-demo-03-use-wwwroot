using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool Visibility { get; set; }
    }
}
