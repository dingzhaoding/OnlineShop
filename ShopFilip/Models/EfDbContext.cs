using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopFilip.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class EfDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }
        public DbSet<Register> Login { get; set; }
        public DbSet<Product> ProductsData { get; set; }
        public DbSet<ProductAtribute> ProductAtributes { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
   
}
