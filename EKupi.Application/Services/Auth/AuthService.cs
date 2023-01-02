using EKupi.Application.Common;
using EKupi.Application.Customers.Commands;
using EKupi.Application.Interfaces;
using EKupi.Domain.Entities;
using EKupi.Domain.Enums;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ITokenSettings _tokenSettings;
        public AuthService(ITokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }

        public LoginCustomerCommandResponse GetLoggedInUserData(Customer user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(CustomClaimTypes.Username, user.UserName),
                new Claim(CustomClaimTypes.FirstName, user.FirstName),
                new Claim(CustomClaimTypes.FamilyName, user.FamilyName),
                new Claim(CustomClaimTypes.Permissions, PermissionPolicyEnum.User.ToString())
            };

            if (user.UserName == "admin")
            {
                authClaims.Add(new Claim(CustomClaimTypes.Permissions, PermissionPolicyEnum.Admin.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(_tokenSettings.Secret));

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddDays(14),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var permissions = new List<PermissionPolicyEnum>();
            var permissionClaims = authClaims.FindAll(x => x.Type == CustomClaimTypes.Permissions);
            foreach (var permissionClaim in permissionClaims)
            {
                var permission = (PermissionPolicyEnum)Enum.Parse(typeof(PermissionPolicyEnum), permissionClaim.Value);
                permissions.Add(permission);
            }

            var result = new LoginCustomerCommandResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                FamilyName = user.FamilyName,
                Token = tokenString,
                Permissions = permissions
            };

            return result;
        }
    }
}
