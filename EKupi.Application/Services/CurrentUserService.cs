using EKupi.Application.Common;
using EKupi.Application.Services;
using EKupi.Domain.Enums;
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
            Permissions = new List<PermissionPolicyEnum>();

            if (principal != null && principal.Identity.IsAuthenticated)
            {
                UserId = ((ClaimsPrincipal)principal).FindFirstValue(ClaimTypes.NameIdentifier);
                FirstName = ((ClaimsPrincipal)principal).FindFirstValue(CustomClaimTypes.FirstName);
                FamilyName = ((ClaimsPrincipal)principal).FindFirstValue(CustomClaimTypes.FamilyName);

                var permissionClaims = ((ClaimsPrincipal)principal).FindAll(CustomClaimTypes.Permissions);
                foreach(var permissionClaim in permissionClaims)
                {
                    var permission = (PermissionPolicyEnum)Enum.Parse(typeof(PermissionPolicyEnum), permissionClaim.Value);
                    Permissions.Add(permission);
                }
            }
        }

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public IList<PermissionPolicyEnum> Permissions { get; set; }

    }
}
