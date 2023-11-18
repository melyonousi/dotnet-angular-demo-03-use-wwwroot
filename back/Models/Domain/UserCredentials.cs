namespace back.Models.Domain
{
    public class UserCredentials
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
