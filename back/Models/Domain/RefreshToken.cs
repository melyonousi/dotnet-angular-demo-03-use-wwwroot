using System.ComponentModel.DataAnnotations;

namespace back.Models.Domain
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public required string RToken { get; set; }
        public bool Status { get; set; }
        public Guid UserId { get; set; }
    }
}
