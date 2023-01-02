using EKupi.Application.Common;
using EKupi.Application.Common.Exceptions;
using EKupi.Application.Customers.Queries;
using EKupi.Application.Interfaces;
using EKupi.Application.Services.Auth;
using EKupi.Domain.Entities;
using EKupi.Domain.Enums;
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
    public class LoginCustomerCommandResponse : UserDTO
    {
        public LoginCustomerCommandResponse()
        {
            Permissions = new List<PermissionPolicyEnum>();
        }
        public string Token { get; set; }
        public IEnumerable<PermissionPolicyEnum> Permissions { get; set; }
    }
    public class LoginCustomerCommand : IRequest<LoginCustomerCommandResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCustomerCommandHandler : IRequestHandler<LoginCustomerCommand, LoginCustomerCommandResponse>
    {
        private readonly UserManager<Customer> _userManager;
        private readonly ITokenSettings _tokenSettings;
        private IHttpContextAccessor _httpContextAccessor;
        private IAuthService _authService;

        public LoginCustomerCommandHandler(
            UserManager<Customer> userManager,
            ITokenSettings tokenSettings,
            IHttpContextAccessor httpContextAccessor,
            IAuthService authService)
        {
            _userManager = userManager;
            _tokenSettings = tokenSettings;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        public async Task<LoginCustomerCommandResponse> Handle(LoginCustomerCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var result = _authService.GetLoggedInUserData(user);

                return result;
            }
            throw new NotFoundException("Invalid credentials");
        }
    }
}
