using EKupi.Application.Customers.Commands;
using EKupi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EKupi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterCustomerCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok();
        }
    }
}
