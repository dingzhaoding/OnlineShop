using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopFilip.Helpers;
using ShopFilip.Models;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly EfDbContext _context;
        public ProductController(EfDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Product(int id)
        {
            ProductViewModel viewModel = ViewModelFactory.MapProductToViewModel(_context.Products.FirstOrDefault(x => x.Id == id));
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel viewModel)
        {
            return RedirectToAction("Buy", "ShoppingCart", new { id = viewModel.Id });
        }

        public async Task<IActionResult> Show(string genre)
        {
            List<Product> products;
            if (!string.IsNullOrEmpty(genre))
            {
                products = await _context.Products.Where(m => string.Equals(m.Name, genre, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
            }
            else
            {
                products = await _context.Products.ToListAsync();
            }
            if (products == null)
            {
                return NotFound();
            }
            ViewData["Genre"] = genre;
            return View(products);
        }
    }
}