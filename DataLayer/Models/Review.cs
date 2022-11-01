using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }

        public Review(Guid id, Guid productId, double rating, string description)
        {
            Id = id;
            ProductId = productId;
            Rating = rating;
            Description = description;
        }
    }
}
