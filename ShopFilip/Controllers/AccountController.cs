using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopFilip.Models;
using ShopFilip.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShopFilip.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signManager;
        private EfDbContext _context;

        public AccountController(EfDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            _context = context;
            _userManager = userManager;
            _signManager = signManager;
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username ,Email=model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(user, false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            //foreach (var item in errors)
            //{

            //}
            return View();
        }

        public IActionResult MainPage()
        {
            if (User.IsInRole("Admin"))
                return this.RedirectToAction("Index", "Admin");
            else if (User.IsInRole("Member"))
                return this.RedirectToAction("Manage", "Account");
            else
                return RedirectToAction("MainPage", "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new Register{ ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("MainPage", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {

            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.Username,
                   model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return this.RedirectToAction("MainPage", "Account");
                    }

                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var item in errors)
            {

            }
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        public async Task<IActionResult> Index(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ViewBag.Email = user.Email;
            return View();
        }
        
        public IActionResult ManageAccount()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Users.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id,Register model)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index", new { id =id});
        }
    }
}