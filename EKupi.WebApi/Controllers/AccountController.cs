using EKupi.Application.Customers.Commands;
using EKupi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginCustomerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }
    }
}
