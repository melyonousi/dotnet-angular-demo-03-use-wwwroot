using back.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace back.Handler
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly string _authorizationKey = "Authorization";
        private readonly AppDbContext _appDbContext;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AppDbContext appDbContext)
            : base(options, logger, encoder, clock)
        {
            _appDbContext = appDbContext;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(_authorizationKey))
            {
                return AuthenticateResult.Fail("No Header Found");
            }

            var _headerValue = AuthenticationHeaderValue.Parse(Request.Headers[_authorizationKey]);
            
            var bytes = Convert.FromBase64String(_headerValue.Parameter!);
            string credentials = Encoding.UTF8.GetString(bytes);

            if (string.IsNullOrEmpty(credentials))
            {
                return AuthenticateResult.Fail("UnAuthorized");
            }

            string[] array = credentials.Split(":");
            string email = array[0];
            string password = array[1];
            var user = _appDbContext.Users.FirstOrDefault(item =>
            item.Email == email &&
            item.Password == password);
            if (user == null)
            {
                return AuthenticateResult.Fail("UnAuthorized");
            }

            var claim = new[] { new Claim(ClaimTypes.Email, email) };
            var identity = new ClaimsIdentity(claim, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
