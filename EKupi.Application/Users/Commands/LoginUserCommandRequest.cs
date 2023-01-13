using EKupi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Users.Commands
{
    public class LoginUserCommandRequest : IRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest>
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfigurationRoot _configuration;
        private IHttpContextAccessor _httpContextAccessor;

        public LoginUserCommandHandler(UserManager<User> userManager, IConfigurationRoot configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (user != null && checkPassword)
            {
                var issuer = _configuration["Jwt:Issuer"];

                var audience = _configuration["Jwt:Audience"];

                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                _httpContextAccessor.HttpContext.Response.Cookies.Append("authCookie", jwtToken, new CookieOptions()
                {
                    IsEssential = true,
                    HttpOnly = true,
                    Secure = false,
                });
            }
            return Unit.Value;
        }
    }
}
