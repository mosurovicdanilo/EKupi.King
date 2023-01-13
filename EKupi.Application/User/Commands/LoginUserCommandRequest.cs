using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.User.Commands
{
    public class LoginUserCommandRequest : IRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest>
    {
        private readonly UserManager<Domain.Entities.User> _userManager;

        public LoginUserCommandHandler(UserManager<Domain.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (user != null && checkPassword)
            {

            }

            return Unit.Value;
        }
    }
}
