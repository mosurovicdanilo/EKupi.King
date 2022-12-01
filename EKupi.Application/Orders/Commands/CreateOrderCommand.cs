using EKupi.Application.Common.Exceptions;
using EKupi.Application.Services;
using EKupi.Domain.Entities;
using EKupi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EKupi.Shared;

namespace EKupi.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest
    {
        public CreateOrderCommand()
        {
            OrderDetails = new List<OrderDetailDto>();
        }
        public string? CustomerId { get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }

    public class OrderDetailDto : IOrderDetailDto
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
            var customer = _context.Customers.FirstOrDefault(x => x.Id == request.CustomerId);

            if(customer == null)
            {
                throw new NotFoundException("Invalid customer");
            }

            var order = new Order
            {
                CustomerId = request.CustomerId ?? _currentUserService.UserId,
                OrderNumber = Guid.NewGuid(),
                OrderDate = DateTime.Now
            };

            var products = await _context.Products.Where(x => request.OrderDetails.Select(od => od.ProductId).Contains(x.Id))
                .ToListAsync();

            var productsWithInsufficientUnitsInStock = products.Where(x => x.UnitsInStock - request.OrderDetails.Where(y => y.ProductId == x.Id).Sum(u => u.Quantity) < 0);
            if (productsWithInsufficientUnitsInStock.Any())
            {
                var error = "Following products have insufficient units in stock: " + string.Join(',', products.Select(x => x.Name).ToArray());
                throw new ForbiddenException(error);
            }

            foreach (var detail in request.OrderDetails)
            {
                var product = products.FirstOrDefault(x => x.Id == detail.ProductId);

                if (product == null)
                {
                    throw new NotFoundException("Product not found");
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
