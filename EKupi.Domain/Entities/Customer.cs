using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Domain.Entities
{
    public class Customer : IdentityUser
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public Customer()
        {
            Orders = new List<Order>();
        }
    }
}
