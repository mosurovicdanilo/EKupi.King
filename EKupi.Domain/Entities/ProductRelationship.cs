using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Domain.Entities
{
    public class ProductRelationship
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long RelatedProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Product RelatedProduct { get; set; }
    }
}
