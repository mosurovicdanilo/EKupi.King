using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Infrastructure.Interfaces
{
    public interface ITokenSettings
    {
        int ExpirationHours { get; set; }
        string Secret { get; set; }
    }
}
