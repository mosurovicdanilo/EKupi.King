using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Domain.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual ICollection<ProductRelationship> SubProducts { get; set; }
        public virtual ICollection<ProductRelationship> SubProductOf { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public Product()
        {
            SubProducts = new List<ProductRelationship>();
            SubProductOf = new List<ProductRelationship>();
        }
    }
}
