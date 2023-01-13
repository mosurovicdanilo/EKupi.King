using EKupi.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EKupi.WebApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommandRequest command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommandRequest command)
        {
            System.Diagnostics.Debug.WriteLine("\n********\nController\n*********\n");

            return Ok(await _mediator.Send(command));
        }
    }
}
