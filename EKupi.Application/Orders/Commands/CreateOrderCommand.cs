using EKupi.Application.Common.Exceptions;
using EKupi.Application.Services;
using EKupi.Domain.Entities;
using EKupi.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace EKupi.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest
    {
        public CreateOrderCommand()
        {
            OrderDetails = new List<OrderDetailDto>();
        }
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

            var products = await _context.Products.Where(x => request.OrderDetails.Select(od => od.ProductId).Contains(x.Id))
                .ToListAsync();

            var productsWithInsufficientUnitsInStock = products.Where(x => x.UnitsInStock - request.OrderDetails.Where(y => y.ProductId == x.Id).Sum(u => u.Quantity) < 0);
            if (productsWithInsufficientUnitsInStock.Any())
            {
                string error = "Following products have insufficient units in stock: ";
                foreach(var product in productsWithInsufficientUnitsInStock)
                {
                    error = error + ", " + product.Name;
                }
                throw new ForbiddenException(error);
            }

            foreach(var detail in request.OrderDetails)
            {
                var product = products.FirstOrDefault(x => x.Id == detail.ProductId);

                if(product == null)
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
