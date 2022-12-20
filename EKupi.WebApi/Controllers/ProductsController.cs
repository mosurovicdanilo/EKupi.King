using EKupi.Application.Products.Commands;
using EKupi.Application.Products.Queries;
using EKupi.Domain.Enums;
using EKupi.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [AuthorizePermission(PermissionPolicyEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("{isAscending}")]
        public async Task<IActionResult> Products(bool isAscending)
        {
            return Ok(await _mediator.Send(new ProductQuery(isAscending)));
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> Product(long productId)
        {
            return Ok(await _mediator.Send(new ProductByIdQuery(productId)));
        }

        [AuthorizePermission(PermissionPolicyEnum.Admin)]
        [HttpPut]
        public async Task<IActionResult> EditProduct(EditProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [AuthorizePermission(PermissionPolicyEnum.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            return Ok(await _mediator.Send(new DeleteProductCommand(id)));
        }

        [AuthorizePermission(PermissionPolicyEnum.Admin)]
        [HttpGet("list/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetProductList(int pageNumber, int pageSize)
        {
            return Ok(await _mediator.Send(new ProductListQuery(pageNumber, pageSize)));
        }
    }
}
