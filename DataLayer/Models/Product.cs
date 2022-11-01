using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    internal class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public List<ProductImage> Images { get; set; }

        public Product(Guid id, string name, string description, int amount, List<ProductImage> images)
        {
            Id = id;
            Name = name;
            Description = description;
            Amount = amount;
            Images = images;
        }

        public Product()
        {
        }
    }
}
