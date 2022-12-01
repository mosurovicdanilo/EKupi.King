using EKupi.Application.Common;
using EKupi.Application.Interfaces;
using EKupi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EKupi.Application.Customers.Commands
{
    public class LoginCustomerCommand : IRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCustomerCommandHandler : IRequestHandler<LoginCustomerCommand>
    {
        private readonly UserManager<Customer> _userManager;
        private readonly ITokenSettings _tokenSettings;
        private IHttpContextAccessor _httpContextAccessor;

        public LoginCustomerCommandHandler(
            UserManager<Customer> userManager,
            ITokenSettings tokenSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenSettings = tokenSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(LoginCustomerCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(CustomClaimTypes.FirstName, user.FirstName),
                    new Claim(CustomClaimTypes.FamilyName, user.FamilyName)
                };

                var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(_tokenSettings.Secret));

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(_tokenSettings.ExpirationHours),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                _httpContextAccessor.HttpContext.Response.Cookies.Append("authCookie", tokenString, new CookieOptions()
                {
                    IsEssential = true,
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                });
            }
            return Unit.Value;
        }
    }
}
