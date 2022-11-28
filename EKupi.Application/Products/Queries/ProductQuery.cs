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
            SubProducts = new List<string>();
        }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public IEnumerable<string> SubProducts { get; set; }
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
            var result = await _context.Products.Where(p => !p.IsDeleted)
                .Select(p => new ProductQueryResponse
                {
                    Name = p.Name,
                    CategoryName = p.Category.Name,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    SubProducts = p.SubProducts.Select(sp => sp.RelatedProduct.Name)
                }).SortBy(request.IsAscending, x => x.UnitPrice).ToListAsync(cancellationToken);

            return result;
        }
    }
}