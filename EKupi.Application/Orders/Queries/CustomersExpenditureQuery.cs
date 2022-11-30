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
    public class CustomersExpenditureResponse
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
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
                FirstName = x.FirstName,
                FamilyName = x.FamilyName,
                Total = x.Orders.SelectMany(o => o.OrderDetails).Sum(x => x.Price)
            }).OrderByDescending(x => x.Total)
            .ToListAsync();

            return result;
        }
    }
}
