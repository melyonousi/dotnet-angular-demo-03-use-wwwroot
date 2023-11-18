using back.Models.Domain;

namespace back.Repositories.Interface
{
    public interface IUserRepository
    {
        public Task<User> CreateAsync(User user);
        public Task<User> UpdateAsync(User user);
        public Task<User> DeleteAsync(Guid id);
        public Task<User> UserAsync(Guid id);
        public Task<List<User>> UsersAsync();
        public Task<TokenResponse> Authenticate(UserCredentials user);
        public Task<TokenResponse> RefresshToken(TokenResponse _tokenResponse);
    }
}
