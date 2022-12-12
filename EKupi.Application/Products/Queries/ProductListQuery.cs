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
    public class ProductListQueryResponse
    {
        public ProductListQueryResponse()
        {
            Products = new List<ProductDTO>();
        }
        public IEnumerable<ProductDTO> Products { get; set; }
        public int TotalItems { get; set; }
    }

    public class ProductDTO
    {
        public ProductDTO()
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

    public class ProductListQuery : IRequest<ProductListQueryResponse>
    {
        public ProductListQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class ProductListHandler : IRequestHandler<ProductListQuery, ProductListQueryResponse>
    {
        private readonly IApplicationDbContext _context;
        public ProductListHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductListQueryResponse> Handle(ProductListQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Products.Where(p => !p.IsDeleted)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = p.Category.Name,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    SubProducts = p.SubProducts.Select(sp => sp.RelatedProduct.Name)
                })
                .OrderBy(x => x.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var total = _context.Products.Count();
            var result = new ProductListQueryResponse { Products = data, TotalItems = total };

            return result;
        }
    }
}
