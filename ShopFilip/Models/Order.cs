using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class Order
    {
        public int ID { get; set; }

        public string DateOfOrder { get; set; }

        public List<Product> Products { get; set; }
    }
}
