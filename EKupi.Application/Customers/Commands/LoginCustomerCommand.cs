using EKupi.Domain.Entities;
using EKupi.Infrastructure.AppSettings;
using EKupi.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EKupi.Application.Customers.Commands
{
    public class LoginCustomerCommand : IRequest<string>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCustomerCommandHandler : IRequestHandler<LoginCustomerCommand, string>
    {
        private readonly UserManager<Customer> _userManager;
        private readonly ITokenSettings _tokenSettings;

        public LoginCustomerCommandHandler(
            UserManager<Customer> userManager,
            ITokenSettings tokenSettings)
        {
            _userManager = userManager;
            _tokenSettings = tokenSettings;
        }

        public async Task<string> Handle(LoginCustomerCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(_tokenSettings.Secret));

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(_tokenSettings.ExpirationHours),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return tokenString;
            }
            return string.Empty;
        }
    }
}
