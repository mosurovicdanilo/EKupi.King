using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Domain.Entities
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public int CategoryTypeId { get; set; }
        public string Name { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<ProductRelationship> ProductRelationship { get; set; }
        public ICollection<ProductRelationship> RelatedProductRelationship { get; set; }



    }
}
