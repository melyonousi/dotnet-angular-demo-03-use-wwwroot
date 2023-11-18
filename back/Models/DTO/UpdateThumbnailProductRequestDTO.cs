using System.ComponentModel.DataAnnotations;

namespace back.Models.DTO
{
    public class UpdateThumbnailProductRequestDTO
    {
        [Required]
        public Guid Id { get; set; }
        public required string Thumbnail { get; set; }
    }
}
