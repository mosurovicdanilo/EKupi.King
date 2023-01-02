using EKupi.Application.Customers.Commands;
using EKupi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Services.Auth
{
    public interface IAuthService
    {
        LoginCustomerCommandResponse GetLoggedInUserData(Customer user);
    }
}
