using EKupi.Application.Customers.Commands;
using EKupi.Application.Customers.Queries;
using EKupi.Application.Products.Queries;
using EKupi.Domain.Entities;
using EKupi.Domain.Enums;
using EKupi.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCustomerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCustomerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [AuthorizePermission(PermissionPolicyEnum.User)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("authCookie");
            return Ok();
        }

        [AuthorizePermission(PermissionPolicyEnum.Admin)]
        [HttpGet("users/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetProductList(int pageNumber, int pageSize)
        {
            return Ok(await _mediator.Send(new UserListQuery(pageNumber, pageSize)));
        }

        [AuthorizePermission(PermissionPolicyEnum.User)]
        [HttpGet("user/{userid?}")]
        public async Task<IActionResult> GetUser(string? userId = null)
        {
            return Ok(await _mediator.Send(new UserQuery(userId)));
        }

        [AuthorizePermission(PermissionPolicyEnum.User)]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(await _mediator.Send(new CurrentUserQuery()));
        }
    }
}
