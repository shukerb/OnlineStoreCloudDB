using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTO
{
    public class ReviewDTO
    {
        public string ProductID { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
    }
}
