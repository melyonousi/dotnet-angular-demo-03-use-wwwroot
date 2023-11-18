using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace back.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public required string RoleKey { get; set; }
        public required string RoleName { get; set; }
    }
}
