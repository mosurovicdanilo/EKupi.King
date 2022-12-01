using EKupi.Application.Common;
using EKupi.Application.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                UserId = ((ClaimsPrincipal)principal).FindFirstValue(ClaimTypes.NameIdentifier);
                FirstName = ((ClaimsPrincipal)principal).FindFirstValue(CustomClaimTypes.FirstName);
                FamilyName = ((ClaimsPrincipal)principal).FindFirstValue(CustomClaimTypes.FamilyName);
            }
        }

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }

    }
}
