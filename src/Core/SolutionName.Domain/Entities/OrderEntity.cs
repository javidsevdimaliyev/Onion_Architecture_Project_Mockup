using SolutionName.Domain.Entities.Common;

namespace SolutionName.Domain.Entities
{
    public class OrderEntity : AuditableEntity<int>
    {
        //public Guid CustomerId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public string OrderCode { get; set; }      
        public ICollection<ProductEntity> Products { get; set; }
        public CustomerEntity Customer { get; set; }

        //public Basket Basket { get; set; }
        //public CompletedOrder CompletedOrder { get; set; }
    }
}
