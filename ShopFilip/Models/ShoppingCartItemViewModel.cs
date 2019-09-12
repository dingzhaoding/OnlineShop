using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class ShoppingCartItemViewModel
    {
        public int Quantity { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
