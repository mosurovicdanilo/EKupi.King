using EKupi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Customers.Queries
{
    public class UserListQueryResponse
    {
        public UserListQueryResponse()
        {
            Users = new List<UserDTO>();
        }
        public IEnumerable<UserDTO> Users { get; set; }
        public int TotalItems { get; set; }
    }

    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
    }

    public class UserListQuery : IRequest<UserListQueryResponse>
    {
        public UserListQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class ProductListHandler : IRequestHandler<UserListQuery, UserListQueryResponse>
    {
        private readonly IApplicationDbContext _context;
        public ProductListHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserListQueryResponse> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Customers
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    FamilyName = u.FamilyName,
                })
                .OrderBy(x => x.UserName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var total = _context.Customers.Count();
            var result = new UserListQueryResponse { Users = data, TotalItems = total };

            return result;
        }
    }
}
