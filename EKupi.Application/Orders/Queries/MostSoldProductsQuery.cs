using EKupi.Application.Products.Queries;
using EKupi.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Orders.Queries
{
    public class MostSoldProductsQueryResponse
    {
        public string Name { get; set; }
        public long Quantity { get; set; }
    }

    public class MostSoldProductsQuery : IRequest<IEnumerable<MostSoldProductsQueryResponse>>
    {

    }

    public class MostSoldProductsQueryHandler : IRequestHandler<MostSoldProductsQuery, IEnumerable<MostSoldProductsQueryResponse>>
    {
        private readonly IApplicationDbContext _context;
        public MostSoldProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MostSoldProductsQueryResponse>> Handle(MostSoldProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Products.Where(x => x.OrderDetails.Any())
                .Select(x => new MostSoldProductsQueryResponse
                {
                    Name = x.Name,
                    Quantity = x.OrderDetails.Select(y => y.Quantity).Sum(),
                }).OrderByDescending(x => x.Quantity)
                .Take(10)
                .ToListAsync();

            return result;
        }
    }
}
