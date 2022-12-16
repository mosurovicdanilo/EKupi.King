using EKupi.Application.Common.Exceptions;
using EKupi.Application.Interfaces;
using EKupi.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Customers.Queries
{
    public class UserQueryResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
    }

    public class UserQuery : IRequest<UserQueryResponse>
    {
        public UserQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }

    public class UserQueryHandler : IRequestHandler<UserQuery, UserQueryResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UserQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<UserQueryResponse> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            var user = _context.Customers.Where(x => x.Id == (request.UserId ?? _currentUserService.UserId));
            if(user != null)
            {
                var result = await user.Select(x => new UserQueryResponse
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    FamilyName = x.FamilyName
                }).ToListAsync();

                return result.First();
            }

            throw new NotFoundException("Invalid user id!");
        }
    }
}
