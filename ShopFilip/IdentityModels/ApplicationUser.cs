using Microsoft.AspNetCore.Identity;
using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.IdentityModels
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string PostalCode { get; set; }
        public List<Order> OrdersList { get; set; }
    }
}
