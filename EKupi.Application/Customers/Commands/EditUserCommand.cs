using EKupi.Application.Common.Exceptions;
using EKupi.Application.Interfaces;
using EKupi.Application.Orders.Commands;
using EKupi.Application.Services;
using EKupi.Application.Services.Auth;
using EKupi.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Customers.Commands
{
    public class EditUserCommand : IRequest<LoginCustomerCommandResponse>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
    }

    public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
    {
        public EditUserCommandValidator()
        {
            RuleFor(c => c.UserName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.OldPassword)
                .NotEmpty();

            RuleFor(c => c.NewPassword)
                .NotEmpty()
                .MinimumLength(5)
                .NotEqual(c => c.OldPassword);

            RuleFor(c => c.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.FamilyName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, LoginCustomerCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<Customer> _userManager;
        private readonly IAuthService _authService;

        public EditUserCommandHandler(
            IApplicationDbContext context, 
            UserManager<Customer> userManager,
            IAuthService authService)
        {
            _context = context;
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<LoginCustomerCommandResponse> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (user == null && !await _userManager.CheckPasswordAsync(user, request.OldPassword))
            {
                throw new NotFoundException("Invalid credentials");
            }

            if(_context.Customers.Where(x => x.UserName == request.UserName).Any())
            {
                throw new ForbiddenException($"Username {request.UserName} already exists!");
            }

            if(request.NewPassword != null)
            {
                await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            }

            user.UserName = request.UserName;
            user.FirstName = request.FirstName;
            user.FamilyName = request.FamilyName;

            await _context.SaveChangesAsync(cancellationToken);

            var result = _authService.GetLoggedInUserData(user);

            return result;
        }
    }
}
