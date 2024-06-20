using SolutionName.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Domain.Entities
{
    public class ProductEntity : AuditableEntity<int>
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }

        public ICollection<OrderEntity> Orders { get; set; }
        //public ICollection<ProductImageFile> ProductImageFiles { get; set; }
        //public ICollection<BasketItem> BasketItems { get; set; }
    }
}
