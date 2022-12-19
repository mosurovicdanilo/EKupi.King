using EKupi.Application.Common.Exceptions;
using EKupi.Application.Extensions;
using EKupi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Products.Queries
{
    public class ProductByIdQueryResponse
    {
        public ProductByIdQueryResponse()
        {
            SubProducts = new List<string>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public IEnumerable<string> SubProducts { get; set; }
    }

    public class ProductByIdQuery : IRequest<ProductByIdQueryResponse>
    {
        public ProductByIdQuery(long productId)
        {
            ProductId = productId;
        }
        public long ProductId { get; set; }
    }

    public class ProductByIdHandler : IRequestHandler<ProductByIdQuery, ProductByIdQueryResponse>
    {
        private readonly IApplicationDbContext _context;
        public ProductByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductByIdQueryResponse> Handle(ProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.Include(x => x.Category).Include(x => x.SubProducts).FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if(product != null)
            {
                var result = new ProductByIdQueryResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    CategoryName = product.Category.Name,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    SubProducts = product.SubProducts.Select(sp => sp.RelatedProduct.Name)
                };
                return result;
            }

            throw new NotFoundException("Invalid product id");
        }
    }
}
