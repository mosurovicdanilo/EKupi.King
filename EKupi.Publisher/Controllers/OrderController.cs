using EKupi.Publisher.DTOs;
using EKupi.Shared;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKupi.Publisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            await _publishEndpoint.Publish<IOrderCreated>(new
            {
                orderDto.CustomerId,
                orderDto.OrderDetails
            });
            return Ok();
        }
    }
}
