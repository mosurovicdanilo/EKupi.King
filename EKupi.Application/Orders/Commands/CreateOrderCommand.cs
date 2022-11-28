using EKupi.Application.Products.Commands;
using EKupi.Application.Services;
using EKupi.Domain.Entities;
using EKupi.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest
    {
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }

    public class OrderDetailDto
    {
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class RegisterCustomerCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public RegisterCustomerCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                CustomerId = "05d7a662-40e1-4d24-a90b-56488de2bc8c",
                OrderNumber = Guid.NewGuid(),
                OrderDate = DateTime.Now
            };
            /*foreach(OrderDetailDto detail in request.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = detail.ProductId,
                    Price = detail.Price,
                    Quantity = detail.Quantity,
                    Total = detail.Price * detail.Quantity
                });
            }*/
            _context.Orders.Add(order);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
