using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace back.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Credentials { get; set; }
        public string? Token { get; set; }
        public string? Avatar { get; set; }
        public Guid? RoleId { get; set; }
    }
}
