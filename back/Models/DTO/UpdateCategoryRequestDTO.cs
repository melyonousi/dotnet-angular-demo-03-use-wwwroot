using System.ComponentModel.DataAnnotations;

namespace back.Models.DTO
{
    public class UpdateCategoryRequestDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? UrlHandle { get; set; }
    }
}
