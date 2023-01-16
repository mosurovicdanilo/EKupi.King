using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Domain.Entities
{
    public class Order
    {
        [Key]
        public long Id { get; set; }
        public string CustomerId { get; set; }
        public Guid OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual User User { get; set; }
    }
}
