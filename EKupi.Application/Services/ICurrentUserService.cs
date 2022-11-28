using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Services
{
    public interface ICurrentUserService
    {
        string UserId { get; set; }
        string FirstName { get; set; }
        string FamilyName { get; set; }
    }
}
