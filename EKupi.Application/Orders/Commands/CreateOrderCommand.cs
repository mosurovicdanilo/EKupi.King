using EKupi.Application.Services;
using EKupi.Domain.Entities;
using EKupi.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


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
                CustomerId = _currentUserService.UserId,
                OrderNumber = Guid.NewGuid(),
                OrderDate = DateTime.Now
            };
            foreach(OrderDetailDto detail in request.OrderDetails)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == detail.ProductId, cancellationToken);

                if(product == null)
                {
                    throw new Exception();
                }

                if((product.UnitsInStock - detail.Quantity) < 0)
                {
                    throw new Exception();
                }

                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = detail.ProductId,
                    Price = detail.Price,
                    Quantity = detail.Quantity,
                    Total = detail.Price * detail.Quantity
                });
                product.UnitsInStock = product.UnitsInStock -= detail.Quantity;
            }
            _context.Orders.Add(order);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
