using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopFilip.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ShopFilip.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using static ShopFilip.Helpers.DataPayU;
using Newtonsoft.Json;
using System.Net;
using ShopFilip.IdentityModels;

namespace OnlineShop.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private EfDbContext _context;
        public string accessToken { get; set; }
        public string Uri { get; set; }

        public CartController(EfDbContext context)
        {
            _context = context;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            return View();
        }

        [Route("buy/{id}")]
        public async Task<IActionResult> Buy(int id)
        {
            if (SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                var productModel = await _context.ProductsData.FindAsync(id);
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = productModel, Quantity = 1 });
                SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    var productModel = await _context.ProductsData.FindAsync(id);

                    cart.Add(new Item { Product = productModel, Quantity = 1 });
                }
                SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<Item> cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Item> cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        [HttpGet]
        public async Task<IActionResult> myAction(string id,int Price)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userProp = await _context.Users.FindAsync(id);
            
            await GetAccessTokenAsync(userProp, Price);
           
            return Redirect(Uri);
        }
       

        public async Task GetAccessTokenAsync(ApplicationUser user, int Price)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://secure.payu.com/pl/standard/user/oauth/authorize"))
                {
                    request.Headers.TryAddWithoutValidation("Host", "secure.payu.com");
                    request.Content = new StringContent("grant_type=client_credentials&client_id=145227&client_secret=12f071174cb7eb79d4aac5bc2f07563f", Encoding.UTF8, "application/x-www-form-urlencoded");
                    try
                    {
                        var response = await httpClient.SendAsync(request);
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var objResponse1 = JsonConvert.DeserializeObject<RootObject2>(jsonString);
                        accessToken = objResponse1.access_token;

                        await Order(user.Email, Price);
                        Response.Redirect(Uri);
                    }
                    catch (Exception)
                    {
                        ErrorPage();
                    }
                  
                }
            }
        }

        [Route("ErrorPage")]
        private IActionResult ErrorPage()
        {
            return View();
        }

        public async Task Order(string name,int price)
        {
            string ProperPrice = price.ToString();
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://secure.payu.com/api/v2_1/orders"))
                {
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer "+accessToken+"");

                    request.Content = new StringContent("{\n    \"notifyUrl\": \"https://your.eshop.com/notify\",\n    \"customerIp\": " +
                        "\"127.0.0.1\",\n    \"merchantPosId\": \"145227\",\n    \"description\": \"RTV market\",\n   " +
                        " \"currencyCode\": \"PLN\",\n    \"totalAmount\": \""+ProperPrice+"\",\n    \"buyer\": {\n       " +
                        " \"email\": \""+name+"\",\n        \"phone\": \"654111654\",\n        \"firstName\": \"John\",\n  " +
                        "      \"lastName\": \"Doe\",\n        \"language\": \"pl\"\n    },\n    \"products\": [\n      " +
                        "  {\n            \"name\": \"Wireless Mouse for Laptop\",\n            \"unitPrice\": \""+ProperPrice+"\",\n    " +
                        "" +
                        "" +
                        "        \"quantity\": \"1\"\n        },\n        {\n            \"name\": \"HDMI cable\",\n        " +
                        "    \"unitPrice\": \"6000\",\n            \"quantity\": \"1\"\n        }\n    ]\n}", Encoding.UTF8, "application/json");
                    var response = await httpClient.SendAsync(request);
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var objResponse1 = JsonConvert.DeserializeObject<RootObject>(jsonString);

                    Uri = objResponse1.redirectUri;
                }
            }
        }
    }
}
