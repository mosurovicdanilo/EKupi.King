using EKupi.Application.Orders.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKupi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/product")]
        public async Task<IActionResult> AddProduct(AddProductCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
