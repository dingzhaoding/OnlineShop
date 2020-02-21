using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class PaginationModel
    {
        public List<Product> Products { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordCount { get; set; }
    }
}
