using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopFilip;
using ShopFilip.Helpers;
using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if (id != 0)
            {
                ProductViewModel viewModel = ViewModelFactory.MapProductToViewModel(_context.Products.FirstOrDefault(x => x.Id == id));
                return View(viewModel);
            }
            return View();
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

        [Route("MenMainPage")]
        public async Task<IActionResult> MenMainPage(int? pageNumber)
        {
            return View();
        }

        [Route("WomanMainPage")]
        public async Task<IActionResult> WomanMainPage()
        {
            return View(await _context.Products.ToListAsync());
        }

        //public async Task<IActionResult> ButtonClick(int? pageNumber)
        //{
        //    string fruit = HttpContext.Request.Form["testSelect"];
        //    var students = from s in _context.Products
        //                   select s;
        //    int pageSize = 1;
        //    return View(await PaginatedList<Product>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
        //    //return View(await _context.Products.ToListAsync());
        //}

        public ActionResult GetPaggedData(string SearchBy, string SearchValue, int pageNumber = 1, int pageSize = 2)
        {
            //List<Product> listData = _context.Products.ToList();
            List<Product> list = new List<Product>();
            if (SearchBy == "Name")
            {
                try
                {
                    list.AddRange(from o in _context.Products
                            where o.Name == SearchValue || SearchValue == null
                            select o);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not name", SearchValue);
                }
                var pagedData = Pagination.PagedResult(list, pageNumber, pageSize);
                return Json(pagedData);
            }
            else if(SearchValue=="")
            {
                list.AddRange(_context.Products.ToList());
                var pagedData = Pagination.PagedResult(list, pageNumber, pageSize);
                return Json(pagedData);
            }
            else
            {
                list.AddRange(from o in _context.Products
                              where o.Group == SearchValue || SearchValue == null
                              select o);
                var pagedData = Pagination.PagedResult(list, pageNumber, pageSize);
                return Json(pagedData);
            }
        }

        //public async Task<JsonResult> GetSearchingData(string SearchBy, string SearchValue, int? pageNumber)
        //{
        //    IQueryable<Product> ProdList=null;
        //    int pageSize = 1;
        //    if (SearchBy=="Name")
        //    {
        //        try
        //        {
        //            ProdList = (from o in _context.Products
        //                        where o.Name == SearchValue || SearchValue==null
        //                        select o);
        //        }
        //        catch (FormatException)
        //        {
        //            Console.WriteLine("{0} is not name",SearchValue);
        //        }
        //        return Json(await PaginatedList<Product>.CreateAsync(ProdList.AsNoTracking(), pageNumber ?? 1, pageSize));
        //    }
        //    else
        //    {
        //        ProdList = (from o in _context.Products
        //                        where o.Group== SearchValue || SearchValue==null
        //                        select o);
        //        return Json(await PaginatedList<Product>.CreateAsync(ProdList.AsNoTracking(), pageNumber ?? 1, pageSize));
        //    }
        //}

        [HttpPost]
        public JsonResult AjaxMethod(int pageIndex, string sortName, string sortDirection)
        {
            PaginationModel model = new PaginationModel();
            model.PageIndex = pageIndex;
            model.PageSize = 10;
            model.RecordCount = _context.Products.Count();
            int startIndex = (pageIndex - 1) * model.PageSize;

            switch (sortName)
            {
                case "CustomerID":
                case "":
                    if (sortDirection == "ASC")
                    {
                        model.Products = (from customer in _context.Products
                                          select customer)
                                .OrderBy(customer => customer.Id)
                                .Skip(startIndex)
                                .Take(model.PageSize).ToList();
                    }
                    else
                    {
                        model.Products = (from customer in _context.Products
                                          select customer)
                                .OrderByDescending(customer => customer.Id)
                                .Skip(startIndex)
                                .Take(model.PageSize).ToList();
                    }
                    break;
                case "ContactName":
                    if (sortDirection == "ASC")
                    {
                        model.Products = (from customer in _context.Products
                                          select customer)
                                .OrderBy(customer => customer.Group)
                                .Skip(startIndex)
                                .Take(model.PageSize).ToList();
                    }
                    else
                    {
                        model.Products = (from customer in _context.Products
                                          select customer)
                                .OrderByDescending(customer => customer.Group)
                                .Skip(startIndex)
                                .Take(model.PageSize).ToList();
                    }
                    break;
                case "City":
                    if (sortDirection == "ASC")
                    {
                        model.Products = (from customer in _context.Products
                                          select customer)
                                .OrderBy(customer => customer.Name)
                                .Skip(startIndex)
                                .Take(model.PageSize).ToList();
                    }
                    else
                    {
                        model.Products = (from customer in _context.Products
                                          select customer)
                                .OrderByDescending(customer => customer.Name)
                                .Skip(startIndex)
                                .Take(model.PageSize).ToList();
                    }
                    break;
            }

            return Json(model);
        }
    }
}