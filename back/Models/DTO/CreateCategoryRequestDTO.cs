using System.ComponentModel.DataAnnotations;

namespace back.Models.DTO
{
    public class CreateCategoryRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? UrlHandle { get; set; }
    }
}
