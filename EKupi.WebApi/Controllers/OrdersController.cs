using EKupi.Application.Hubs;
using EKupi.Application.Orders.Commands;
using EKupi.Application.Orders.Queries;
using EKupi.Domain.Enums;
using EKupi.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace EKupi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<GuidGeneratorHub> _hub;

        public OrdersController(IMediator mediator, IHubContext<GuidGeneratorHub> hub)
        {
            _mediator = mediator;
            _hub = hub;
        }

        [AuthorizePermission(PermissionPolicyEnum.User)]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetOrders(string customerId)
        {
            return Ok(await _mediator.Send(new OrderQuery(customerId)));
        }

        [AuthorizePermission(PermissionPolicyEnum.User)]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            return Ok(await _mediator.Send(new DeleteOrderCommand(id)));
        }

        [AuthorizePermission(PermissionPolicyEnum.Admin)]
        [HttpGet("mostSoldProducts")]
        public async Task<IActionResult> GetMostSoldProducts()
        {
            return Ok(await _mediator.Send(new MostSoldProductsQuery()));
        }

        [AuthorizePermission(PermissionPolicyEnum.Admin)]
        [HttpGet("customerExpenditure")]
        public async Task<IActionResult> GetCustomerExpenditure()
        {
            return Ok(await _mediator.Send(new CustomersExpenditureQuery()));
        }

        //[AuthorizePermission(PermissionPolicyEnum.Admin)]
        [HttpGet("productSalesPerMonth")]
        public async Task<IActionResult> GetProductSalesPerMonth()
        {
            return Ok(await _mediator.Send(new ProductSalesPerMonthQuery()));
        }

        [HttpPut]
        public async Task<IActionResult> EditOrder(EditOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("testsignalr")]
        [AllowAnonymous]
        public async Task<IActionResult> TestHub()
        {
            foreach (var item in Enumerable.Range(1, 1000))
            {
                await _hub.Clients.All.SendAsync("NewGuid", Guid.NewGuid());
                await Task.Delay(100);
            }
            return Ok();
        }
    }
}
