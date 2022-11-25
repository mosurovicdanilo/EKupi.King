using EKupi.Application.Extensions;
using EKupi.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Products.Queries
{

    public class ProductQueryResponse
    {
        public ProductQueryResponse()
        {
            Products = new List<ProductDto>();
        }
        public string CategoryName { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        public ProductDto()
        {
            SubProducts = new List<SubProductDto>();
        }
        public string Name { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public IEnumerable<SubProductDto> SubProducts { get; set; }
    }

    public class SubProductDto
    {
        public string Name { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ProductQuery : IRequest<IEnumerable<ProductQueryResponse>>
    {
        public ProductQuery(bool isAscending)
        {
            IsAscending = isAscending;
        }
        public bool IsAscending { get; set; }
    }

    public class ProductHandler : IRequestHandler<ProductQuery, IEnumerable<ProductQueryResponse>>
    {
        private readonly IApplicationDbContext _context;
        public ProductHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductQueryResponse>> Handle(ProductQuery request, CancellationToken cancellationToken)
        {
            var SubProducts = _context.ProductRelationships.Select(sp => new SubProductDto
            {
                Name = sp.RelatedProduct.Name,
                UnitsInStock = sp.RelatedProduct.UnitsInStock,
                UnitPrice = sp.RelatedProduct.UnitPrice
            }).SortBy(request.IsAscending, sp => sp.UnitPrice);

            var products = _context.Products.Select(p => new ProductDto
            {
                Name = p.Name,
                UnitsInStock = p.UnitsInStock,
                UnitPrice = p.UnitPrice,
            }).SortBy(request.IsAscending, p => p.UnitPrice);

            var combined = _context.Products.Select(p => new ProductDto
            {
                Name = p.Name,
                UnitsInStock = p.UnitsInStock,
                UnitPrice = p.UnitPrice,
                SubProducts = p.SubProducts.Select(sp => new SubProductDto
                {
                    Name = sp.RelatedProduct.Name,
                    UnitsInStock = sp.RelatedProduct.UnitsInStock,
                    UnitPrice = sp.RelatedProduct.UnitPrice
                }).SortBy(request.IsAscending, sp => sp.UnitPrice)
            }).SortBy(request.IsAscending, p => p.UnitPrice);

            var result = await _context.Categories.Select(c => new ProductQueryResponse
            {
                CategoryName = c.Name,
                Products = c.Products.Select(p => new ProductDto
                {
                    Name = p.Name,
                    UnitsInStock = p.UnitsInStock,
                    UnitPrice = p.UnitPrice,
                    SubProducts = p.SubProducts.Select(sp => new SubProductDto
                    {
                        Name = sp.RelatedProduct.Name,
                        UnitsInStock = sp.RelatedProduct.UnitsInStock,
                        UnitPrice = sp.RelatedProduct.UnitPrice
                    }).SortBy(request.IsAscending, sp => sp.UnitPrice)
                }).SortBy(request.IsAscending, p => p.UnitPrice)
            }).ToListAsync(cancellationToken);

            return result;
        }
    }
}