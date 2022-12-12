using EKupi.Application.Hubs;
using EKupi.Publisher.DTOs;
using EKupi.Shared;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EKupi.Publisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IHubContext<GuidGeneratorHub> _hub;

        public OrderController(IPublishEndpoint publishEndpoint, IHubContext<GuidGeneratorHub> hub)
        {
            _publishEndpoint = publishEndpoint;
            _hub = hub;
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TestHub()
        {
            foreach (var item in Enumerable.Range(1, 100))
            {
                await _hub.Clients.All.SendAsync("NewGuid", Guid.NewGuid());
                await Task.Delay(100);
            }
            return Ok();
        }
    }
}
