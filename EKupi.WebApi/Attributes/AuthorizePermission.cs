using EKupi.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EKupi.WebApi.Attributes
{
    public class AuthorizePermission : AuthorizeAttribute
    {
        public AuthorizePermission(PermissionPolicyEnum permissionPolicy)
        {
            Policy = Enum.GetName(typeof(PermissionPolicyEnum), permissionPolicy);
        }
    }
}
