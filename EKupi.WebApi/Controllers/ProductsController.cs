using EKupi.Application.Products.Commands;
using EKupi.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKupi.WebApi.Controllers
{
    [Authorize]
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
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("/products")]
        public async Task<IActionResult> Products(bool isAscending)
        {
            return Ok(await _mediator.Send(new ProductQuery(isAscending)));
        }

        [HttpPost("/editProduct")]
        public async Task<IActionResult> EditProduct(EditProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("/product")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCommand(id)));
        }
    }
}
