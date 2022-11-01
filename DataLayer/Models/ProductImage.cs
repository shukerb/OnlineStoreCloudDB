using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    internal class ProductImage
    {
        public string Name { get; set; }
        public string URL { get; set; }

        public ProductImage(string uRL, string name)
        {
            URL = uRL;
            Name = name;
        }
        public ProductImage()
        {

        }
    }
}
