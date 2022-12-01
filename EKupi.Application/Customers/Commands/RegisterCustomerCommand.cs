using EKupi.Application.Common.Exceptions;
using EKupi.Application.Interfaces;
using EKupi.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Customers.Commands
{
    public class RegisterCustomerCommand : IRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
    }

    public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerCommandValidator()
        {
            RuleFor(c => c.UserName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.Password)
                .NotEmpty()
                .MinimumLength(5);

            RuleFor(c => c.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.FamilyName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }

    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<Customer> _userManager;


        public RegisterCustomerCommandHandler(
            IApplicationDbContext context,
            UserManager<Customer> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                FamilyName = request.FamilyName
            };

            var result = await _userManager.CreateAsync(customer, request.Password);

            if (result.Succeeded)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception("Something went wrong while registering");
            }

            return Unit.Value;
        }
    }
}
