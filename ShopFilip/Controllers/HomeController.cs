using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopFilip.Models;

namespace ShopFilip.Controllers
{
    public class HomeController : Controller
    {
        private EfDbContext _context;

        public HomeController(EfDbContext context)
        {
            _context = context;
        }

        [Route("MainPage")]
        public async Task<IActionResult> MainPage()
        {
            return View(await _context.Products.ToListAsync());
        }

        public IActionResult ContactForm()
        {
            return View();
        }
    }
}