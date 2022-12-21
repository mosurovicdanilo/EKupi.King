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
            SubProducts = new List<SubproductDTO>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public IEnumerable<SubproductDTO> SubProducts { get; set; }
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
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if(product != null)
            {
                var result = await _context.Products.Where(p => p.Id == request.ProductId)
                    .Select(p => new ProductByIdQueryResponse
                    {
                        Id = p.Id,
                        Name = p.Name,
                        CategoryName = p.Category.Name,
                        UnitPrice = p.UnitPrice,
                        UnitsInStock = p.UnitsInStock,
                        SubProducts = p.SubProducts.Select(sp => new SubproductDTO
                        {
                            Id = sp.RelatedProduct.Id,
                            Name = sp.RelatedProduct.Name,
                            CategoryName = sp.RelatedProduct.Category.Name,
                            UnitPrice = sp.RelatedProduct.UnitPrice,
                            UnitsInStock = sp.RelatedProduct.UnitsInStock
                        })
                    }).FirstOrDefaultAsync(cancellationToken);
                return result;
            }
            throw new NotFoundException("Invalid product id");
        }
    }
}
