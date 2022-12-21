using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using global::EKupi.Application.Common.Exceptions;
using global::EKupi.Application.Interfaces;
using global::EKupi.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKupi.Domain.Enums;

namespace EKupi.Application.Customers.Queries
{
    public class CurrentUserQueryResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public IEnumerable<PermissionPolicyEnum> Permissions { get; set; }
    }

    public class CurrentUserQuery : IRequest<CurrentUserQueryResponse>
    {
    }

    public class CurrentUserQueryHandler : IRequestHandler<CurrentUserQuery, CurrentUserQueryResponse>
    {
        private readonly ICurrentUserService _currentUserService;

        public CurrentUserQueryHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<CurrentUserQueryResponse> Handle(CurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = new CurrentUserQueryResponse
            {
                Id = _currentUserService.UserId,
                UserName = _currentUserService.Username,
                FirstName = _currentUserService.FirstName,
                FamilyName = _currentUserService.FamilyName,
                Permissions = _currentUserService.Permissions
            };

            return user;
        }
    }
}

