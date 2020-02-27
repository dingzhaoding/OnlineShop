using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopFilip.IdentityModels;
using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Controllers
{
    [RoleFilter(Role = "Admin")]
    public class AdminController : Controller
    {
        private EfDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        private readonly IHostingEnvironment he;

        public AdminController(EfDbContext context, UserManager<ApplicationUser> user, IHostingEnvironment e)
        {
            _context = context;
            _userManager = user;
            he = e;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> AllProducts()
        {
            return View(await _context.ProductsData.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShowFields(string fullName, IFormFile pic)
        {
            ViewData["fname"] = fullName;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product productModel, string gender, string[] size, string fullName, string group, IFormFile pic)
        {
            List<ProductAtribute> prodAtr = new List<ProductAtribute>();
            Product product = new Product();
            if (ModelState.IsValid)
            {
                foreach (var item in size)
                {
                    prodAtr.Add(new ProductAtribute("size", item));
                }

                product = productModel;
                product.Gender = gender;
                product.Group = group;
                product.ProductAtribute = prodAtr;
                _context.Add(product);

                var picName = "";
                if (pic != null)
                {
                    var imagePath = @"\Upload\Images\";
                    var uploadPath = he.WebRootPath + imagePath;


                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString();
                    var fileName = Path.GetFileName(uniqueFileName + "." + pic.FileName.Split(".")[1].ToLower());

                    string fullPath = uploadPath + fileName;
                    imagePath = imagePath + @"\";
                    var filePath =Path.Combine(imagePath, fileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await pic.CopyToAsync(fileStream);
                    }
                    picName = filePath;
                }
                product.Photo = picName;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductsData.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            return View(productModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product productModel)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(productModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productModel);
        }

        private bool ProductModelExists(int id)
        {
            return _context.ProductsData.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductsData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productModel = await _context.ProductsData.FindAsync(id);

            var parent = _context.ProductsData.Include(p => p.ProductAtribute)
                    .SingleOrDefault(p => p.Id == id);

            foreach (var child in parent.ProductAtribute.ToList())
                _context.ProductAtributes.Remove(child);

            _context.ProductsData.Remove(productModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetCustomers()
        {
            var customers = _userManager.Users.ToList();
            return View(customers);
        }
    }
}
