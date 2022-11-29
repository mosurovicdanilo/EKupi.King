using EKupi.Application.Orders.Commands;
using EKupi.Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKupi.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _mediator.Send(new OrderQuery()));
        }

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

        [HttpGet("mostSoldProducts")]
        public async Task<IActionResult> GetMostSoldProducts()
        {
            return Ok(await _mediator.Send(new MostSoldProductsQuery()));
        }

        [HttpGet("customerExpenditure")]
        public async Task<IActionResult> GetCustomerExpenditure()
        {
            return Ok(await _mediator.Send(new CustomersExpenditureQuery()));
        }

        [HttpGet("productSalesPerMonth")]
        public async Task<IActionResult> GetProductSalesPerMonth()
        {
            return Ok(await _mediator.Send(new ProductSalesPerMonthQuery()));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditOrder(EditOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
