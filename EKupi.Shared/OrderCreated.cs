using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Shared
{
    public interface IOrderCreated
    {
        string? CustomerId { get; set; }
        IEnumerable<IOrderDetailDto> OrderDetails { get; set; }
    }

    public interface IOrderDetailDto
    {
        long ProductId { get; set; }
        decimal Price { get; set; }
        int Quantity { get; set; }
    }
}
