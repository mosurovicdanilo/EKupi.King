using EKupi.Application.Customers.Commands;
using EKupi.Domain.Entities;
using EKupi.Infrastructure.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Products.Commands
{
    public class AddProductCommand : IRequest
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public ICollection<long> SubproductIds { get; set; }
    }

    public class RegisterCustomerCommandValidator : AbstractValidator<AddProductCommand>
    {
        public RegisterCustomerCommandValidator()
        {
            RuleFor(p => p.CategoryId).NotEmpty();
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.UnitsInStock).NotEmpty();
            RuleFor(p => p.UnitPrice).NotEmpty();
        }
    }

    public class RegisterCustomerCommandHandler : IRequestHandler<AddProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public RegisterCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                UnitsInStock = request.UnitsInStock,
                UnitPrice = request.UnitPrice,
            };

            foreach(var id in request.SubproductIds)
            {
                product.SubProducts.Add(new ProductRelationship
                {
                    RelatedProductId = id
                });
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
