using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKupi.Domain.Entities;
using EKupi.Infrastructure;
using EKupi.Application.Interfaces;

namespace EKupi.Application.Users.Commands
{
    public class RegisterUserCommandRequest : IRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommandRequest>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(c => c.Username).NotEmpty().Length(4,20);
            RuleFor(c => c.Password).NotEmpty().Length(8, 20);
            RuleFor(c => c.FirstName).NotEmpty().Length(4, 20);
            RuleFor(c => c.LastName).NotEmpty().Length(4, 20);
        }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest>
    {
        private readonly IAppDbContext _context;
        private readonly UserManager<User> _userManager;


        public RegisterUserCommandHandler(
            IAppDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
        {
            System.Diagnostics.Debug.WriteLine("\n********\nIn\n*********\n");
            var user = new User
            {
                UserName = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception("Something went wrong!");
            }

            return Unit.Value;
        }
    }
}
