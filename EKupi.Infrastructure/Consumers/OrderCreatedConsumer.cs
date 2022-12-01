using EKupi.Application.Orders.Commands;
using EKupi.Shared;
using MassTransit;
using MediatR;

namespace EKupi.Infrastructure.Consumers
{
    public class OrderCreatedConsumer : IConsumer<IOrderCreated>
    {
        private readonly IMediator _mediator;

        public OrderCreatedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<IOrderCreated> context)
        {
            var orderDetails = context.Message.OrderDetails.Select(x => new OrderDetailDto
            {
                Price = x.Price,
                Quantity = x.Quantity,
                ProductId = x.ProductId,
            })
                .ToList();

            await _mediator.Send(new CreateOrderCommand
            {
                CustomerId = context.Message.CustomerId,
                OrderDetails = orderDetails
            });
        }
    }
}
