using back.Data;
using back.Handler;
using back.Models.Domain;
using back.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace back.Repositories.Implementation
{
    public class UserRespository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ILogger<UserRespository> _logger;
        private readonly JwtSettings _jwtSettings;

        public UserRespository(AppDbContext appDbContext, IOptions<JwtSettings> options, IRefreshTokenGenerator refreshTokenGenerator, ILogger<UserRespository> logger)
        {
            _appDbContext = appDbContext;
            _refreshTokenGenerator = refreshTokenGenerator;
            _logger = logger;
            _jwtSettings = options.Value;
        }

        [NonAction]
        public async Task<TokenResponse> TokenAuthenticate(Guid id, Claim[] claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddSeconds(20),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey!)),
                    SecurityAlgorithms.HmacSha256)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponse()
            {
                JwtToken = jwtToken,
                RefreshToken = await _refreshTokenGenerator.GenerateToken(id)
            };
        }

        public async Task<TokenResponse> Authenticate(UserCredentials _user)
        {
            var user = await _appDbContext.Users
                            .Where(item => item.Email == _user.Email && item.Password == _user.Password)
                            .Select(item => new
                            {
                                User = item,
                                Role = _appDbContext.Roles.FirstOrDefault(role => role.Id == item.RoleId)
                            })
                            .FirstOrDefaultAsync();

            //var user = await _appDbContext.Users.FirstOrDefaultAsync(item => item.Email == _user.Email && item.Password == _user.Password);

            if (user?.User == null)
            {
                return null!;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            if (_jwtSettings.SecurityKey != null)
            {
                //var role = await _appDbContext.Roles.FindAsync(user.RoleId);
                var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
                var tokenDesc = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.User.Id.ToString()), new Claim(ClaimTypes.Role, user.Role!.RoleKey), new Claim(ClaimTypes.Email, user.User.Email) }),
                    Expires = DateTime.Now.AddSeconds(20),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenHandler.CreateToken(tokenDesc);
                string finalToken = tokenHandler.WriteToken(token);
                user.User.Token = finalToken;
            }

            TokenResponse response = new()
            {
                JwtToken = user.User.Token,
                RefreshToken = await _refreshTokenGenerator.GenerateToken(user.User.Id)
            };

            return response;
        }

        public async Task<TokenResponse> RefresshToken(TokenResponse _tokenResponse)
        {
            var tokenHandler = new JwtSecurityTokenHandler();


            var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey!);

            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(_tokenResponse.JwtToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out securityToken);

            var token = securityToken as JwtSecurityToken;

            if (token != null && !token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return null!;
            }

            Guid id = Guid.Parse(principal.Identity!.Name!);
            _logger.LogError("this is an ID: " + id);

            var user = await _appDbContext.RefreshTokens.FirstOrDefaultAsync(item => item.UserId == id && item.RToken == _tokenResponse.RefreshToken);
            if (user == null)
            {
                return null!;
            }

            var response = TokenAuthenticate(id!, principal.Claims.ToArray()).Result;

            return response;
        }

        public async Task<User> CreateAsync(User user)
        {
            user.RoleId = await _appDbContext.Roles.Where(role => role.RoleKey == "user").Select(role => role.Id).FirstOrDefaultAsync();
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> DeleteAsync(Guid id)
        {
            var usr = await _appDbContext.Users.FindAsync(id);

            if (usr == null)
            {
                return usr!;
            }

            _appDbContext.Users.Remove(usr);
            await _appDbContext.SaveChangesAsync();

            return usr;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var usr = await _appDbContext.Users.FindAsync(user.Id);

            if (usr == null)
            {
                return usr!;
            }

            usr.Name = user.Name;
            usr.Email = user.Email;
            usr.Password = user.Password;
            usr.Avatar = user.Avatar;

            await _appDbContext.SaveChangesAsync();

            return usr;
        }

        public async Task<User> UserAsync(Guid id)
        {
            var usr = await _appDbContext.Users.FindAsync(id);

            if (usr == null)
            {
                return usr!;
            }

            return usr!;
        }

        public async Task<List<User>> UsersAsync()
        {
            var users = await _appDbContext.Users.ToListAsync();
            return users!;
        }
    }
}
