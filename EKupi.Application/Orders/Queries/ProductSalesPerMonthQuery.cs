using EKupi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Orders.Queries
{
    public class ProductSalesPerMonthQueryResponse
    {
        public ProductSalesPerMonthQueryResponse()
        {
            ProductSales = new List<ProductSalesDto>();
        }
        public string Month { get; set; }
        public IEnumerable<ProductSalesDto> ProductSales { get; set; }
    }

    public class ProductSalesDto
    {
        public string Name { get; set; }
        public decimal Total { get; set; }
    }

    public class ProductSalesPerMonthQuery : IRequest<IEnumerable<ProductSalesPerMonthQueryResponse>>
    {

    }

    public class ProductSalesPerMonthQueryHandler : IRequestHandler<ProductSalesPerMonthQuery, IEnumerable<ProductSalesPerMonthQueryResponse>>
    {
        private readonly IApplicationDbContext _context;
        public ProductSalesPerMonthQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductSalesPerMonthQueryResponse>> Handle(ProductSalesPerMonthQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.OrderDetails.GroupBy(x => new
            {
                Month = x.Order.OrderDate.Month,
                Year = x.Order.OrderDate.Year,
            }).Select(x => new ProductSalesPerMonthQueryResponse
            {
                Month = ($"{x.Key.Month}/{x.Key.Year}"),
                ProductSales = x.GroupBy(p => new
                {
                    p.ProductId, 
                    p.Product.Name
                }).Select(x => new ProductSalesDto
                {
                    Name = x.Key.Name,
                    Total = x.Sum(y => y.Total)
                })
            }).ToListAsync();

            return result; 
        }
    }
}
