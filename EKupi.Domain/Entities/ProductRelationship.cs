using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Domain.Entities
{
    public class ProductRelationship
    {
        public long Id { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public long RelatedProductId { get; set; }
        public Product RelatedProduct { get; set; }
    }
}
