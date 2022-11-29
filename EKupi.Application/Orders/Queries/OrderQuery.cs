using EKupi.Application.Services;
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
    public class OrderQueryResponse
    {
        public OrderQueryResponse()
        {
            OrderDetails = new List<OrderDetailsDto>();
        }
        public string CustomerName { get; set; }
        public Guid OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
    }

    public class OrderDetailsDto
    {
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }

    public class OrderQuery : IRequest<IEnumerable<OrderQueryResponse>>
    {
    }

    public class OrderQueryHandler : IRequestHandler<OrderQuery, IEnumerable<OrderQueryResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public OrderQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<IEnumerable<OrderQueryResponse>> Handle(OrderQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Orders.Where(x => x.CustomerId == _currentUserService.UserId)
                .Select(x => new OrderQueryResponse
                {
                    CustomerName = $"{x.Customer.FirstName} {x.Customer.FamilyName}",
                    OrderDate = x.OrderDate,
                    OrderNumber = x.OrderNumber,
                    OrderDetails = x.OrderDetails.Select(od => new OrderDetailsDto
                    {
                        Product = od.Product.Name,
                        Price = od.Price,
                        Quantity = od.Quantity,
                        Total = od.Total,
                    })
                }).ToListAsync(cancellationToken);

            return result;
        }
    }
}
