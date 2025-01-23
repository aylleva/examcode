using ExamMVC.Models;
using ExamMVC.Utilities.Enums;
using ExamMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> usermeneger;
        private readonly SignInManager<AppUser> signUser;
        private readonly RoleManager<IdentityRole> userRole;

        public AccountController(UserManager<AppUser> usermeneger,SignInManager<AppUser> signUser,RoleManager<IdentityRole> userRole)
        {
            this.usermeneger = usermeneger;
            this.signUser = signUser;
            this.userRole = userRole;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM uservm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user =new AppUser() { 
            Name = uservm.Name,
            Surname=uservm.Surname,
            Email= uservm.Email,
            UserName= uservm.UserName
            };

            var result = await usermeneger.CreateAsync(user, uservm.Password);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await usermeneger.AddToRoleAsync(user,UserRoles.Admin.ToString());
            await signUser.SignInAsync(user, false);
            return RedirectToAction(nameof(HomeController.Index), "Home", new { Area = "" });

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM uservm,string? returnurl)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var user = await usermeneger.Users.FirstOrDefaultAsync(u => u.UserName == uservm.UserNameorEmail || u.Email == uservm.UserNameorEmail);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "UserName/Email or Password is incorrect");
                return View();
            }

            var result = await signUser.PasswordSignInAsync(user, uservm.Password, false, true);
            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "UserName/Email or Password is incorrect");
                return View();
            }
            if(result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your Account has been blocked");
                return View();
            }

            if(returnurl is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home", new { Area = "" });
            }
            return Redirect(returnurl);
        }

        public async Task<IActionResult> Logout()
        {
            await signUser.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home", new { Area = "" });
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach(var role in Enum.GetValues(typeof(UserRoles)))
            {
                if(!await userRole.RoleExistsAsync(role.ToString()))
                {
                    await userRole.CreateAsync( new IdentityRole { Name = role.ToString() });
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home", new { Area = "" });
        }
    }
    
}
