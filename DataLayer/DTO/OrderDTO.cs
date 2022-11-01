using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTO
{
    internal class OrderDTO
    {
        public string UserId { get; set; }

        public List<Product> Products { get; set; }

        public DateTime ShippingDate { get; set; }
    }
}
