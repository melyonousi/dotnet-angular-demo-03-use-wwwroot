namespace back.Handler
{
    public interface IRefreshTokenGenerator
    {
        Task<string> GenerateToken(Guid _id);
    }
}
