using back.Data;
using back.Models.Domain;
using System.Security.Cryptography;

namespace back.Handler
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly AppDbContext _appDbContext;

        public RefreshTokenGenerator(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<string> GenerateToken(Guid _id)
        {
            var random_number = new byte[32];
            using (var random_number_generrator = RandomNumberGenerator.Create())
            {
                random_number_generrator.GetBytes(random_number);
                string refresh_token = Convert.ToBase64String(random_number);

                var token = await _appDbContext.RefreshTokens.FindAsync(_id);

                if (token != null)
                {
                    token.RToken = refresh_token;
                }
                else
                {
                    RefreshToken refreshToken = new RefreshToken
                    {
                        RToken = refresh_token,
                        UserId = _id,
                        Status = true
                    };
                    await _appDbContext.RefreshTokens.AddAsync(refreshToken);
                }

                await _appDbContext.SaveChangesAsync();

                return refresh_token;
            };
        }
    }
}
