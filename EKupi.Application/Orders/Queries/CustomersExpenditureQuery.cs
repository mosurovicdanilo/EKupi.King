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
    public class CustomersExpenditureResponse
    {
        public string FullName { get; set; }
        public decimal Total { get; set; }
    }

    public class CustomersExpenditureQuery : IRequest<IEnumerable<CustomersExpenditureResponse>>
    {

    }

    public class CustomersExpenditureQueryHandler : IRequestHandler<CustomersExpenditureQuery, IEnumerable<CustomersExpenditureResponse>>
    {
        private readonly IApplicationDbContext _context;
        public CustomersExpenditureQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomersExpenditureResponse>> Handle(CustomersExpenditureQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Customers.Select(x => new CustomersExpenditureResponse
            {
                FullName = $"{x.FirstName} {x.FamilyName}" ,
                Total = x.Orders.SelectMany(o => o.OrderDetails).Sum(x => x.Total)
            }).OrderByDescending(x => x.Total)
            .ToListAsync();

            return result;
        }
    }
}
