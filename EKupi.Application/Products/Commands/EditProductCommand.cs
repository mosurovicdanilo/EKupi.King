using EKupi.Application.Common.Exceptions;
using EKupi.Domain.Entities;
using EKupi.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Products.Commands
{
    public class EditProductCommand : IRequest
    {
        public EditProductCommand()
        {
            SubproductIds = new List<long>();
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public ICollection<long> SubproductIds { get; set; }
    }

    public class EditProductCommandHandler : IRequestHandler<EditProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public EditProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.Include(p => p.SubProducts).FirstOrDefaultAsync(p => p.Id == request.Id);

            if(product == null)
            {
                throw new NotFoundException("Product not found");
            }

            product.CategoryId = request.CategoryId;
            product.Name = request.Name;
            product.UnitPrice = request.UnitPrice;
            product.UnitsInStock = request.UnitsInStock;

            var addedSubproductIds = request.SubproductIds.Except(product.SubProducts.Select(x => x.RelatedProductId)).ToList();
            var deletedSubproductIds = product.SubProducts.Select(x => x.RelatedProductId).Except(request.SubproductIds).ToList();

            product.SubProducts = product.SubProducts.Where(x => !deletedSubproductIds.Contains(x.RelatedProductId)).ToList();

            foreach (var id in addedSubproductIds)
            {
                product.SubProducts.Add(new ProductRelationship
                {
                    RelatedProductId = id
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
