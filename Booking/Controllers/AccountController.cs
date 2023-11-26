using Booking.Application.Interfaces;
using Booking.Application.Services;
using Booking.Domain.Entities;
using Booking.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace Booking.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(IUnitOfWork unitOfWork,
              UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager,
              RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult GetAll()
        {
            return View();
        }
        public IActionResult Register()
        {
           
            RegisterVM registerVM = new()
            {
                RoleLists = _roleManager.Roles.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                RedirectUrl = Url.Content("~/")
            };
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerVM.Name,
                    PhoneNumber = registerVM.PhoneNumber,
                    Email = registerVM.Email,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedAccount = DateTime.Now,
                };

                var result = _userManager.CreateAsync(user, registerVM.Password).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                       await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    else
                    {
                      await  _userManager.AddToRoleAsync(user, SD.Role_Customer);                
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["success"] = "Account succesfully created";
                    return RedirectToAction("Index", "Home");
                }

            }
            registerVM.RoleLists = _roleManager.Roles.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });


            return View(registerVM);

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {

            var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(loginVM.RedirectUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return LocalRedirect(loginVM.RedirectUrl);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(loginVM);
        }
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            LoginVM loginVM = new()
            {

                RedirectUrl = returnUrl
            };

            return View(loginVM);
        }


    }

}
