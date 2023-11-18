using back.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace back.Models.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Avatar { get; set; }
        public Guid? RoleId { get; set; }
    }
}
