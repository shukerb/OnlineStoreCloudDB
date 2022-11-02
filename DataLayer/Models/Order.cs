using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Product> Products { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public bool IsShipped { get; set; }

        public Order (Guid id, Guid userId, List<Product> products, DateTime creationDate, bool isShipped)
        {
            Id = id;
            UserId = userId;
            Products = products;
            CreationDate = creationDate;
            IsShipped = isShipped;
        }
        public Order()
        {

        }
    }
}
