using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMeinProfile.Models;
using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;

namespace WebMeinProfile.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly MeinProfileContext _context;
        private readonly IWebHostEnvironment _hosting;
        public AccountController(UserManager<AppUser> userManager, IWebHostEnvironment hosting, SignInManager<AppUser> signInManager, MeinProfileContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hosting = hosting;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterView usr, List<IFormFile> file)
        {
            var path = _hosting.WebRootPath + "/upload/";
            if (file.Count > 0)
            {
                var filetarget = path + file[0].FileName;
                using (var stream = new FileStream(filetarget, FileMode.Create))
                {
                    await file[0].CopyToAsync(stream);
                }

                var availUsr = await _userManager.FindByNameAsync(usr.Username);
                if (availUsr != null)
                {
                    ModelState.AddModelError(string.Empty, "Username telah digunakan.");
                    return View(usr);
                }
                if (ModelState.IsValid)
                {
                    // Create user
                    AppUser user = new AppUser
                    {
                        UserName = usr.Username,
                        FullName = usr.FullName,
                        Email    = usr.Email,
                        Age      = usr.Age,
                        Job      = usr.Job,
                        Photo    = file[0].FileName
                    };

                    var result = _userManager.CreateAsync(user, usr.Password).Result;
                    if (result.Succeeded)
                    {
                        return Redirect("/home");
                    }
                }
            }
            return View(usr);
        }
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginView usr, string? returnUrl)
        {

            var usrapp = _userManager.FindByNameAsync(usr.Username).GetAwaiter().GetResult();
            if (usrapp != null)
            {
                _signInManager.SignOutAsync().GetAwaiter().GetResult();
                var hasil = _signInManager.PasswordSignInAsync(usrapp, usr.Password, false, false).GetAwaiter().GetResult();
                if (hasil.Succeeded)
                    return Redirect(returnUrl ?? "/home");
            }
            return View(usr);
        }

        public IActionResult Profil()
        {
            return View();
        }

        public IActionResult ProfileEdit()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProfileEdit(EditView edit)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var logUser = await _userManager.GetUserAsync(User);

                if (!ModelState.IsValid)
                {
                    return View(edit);
                }

                logUser.FullName = edit.FullName;
                logUser.Email = edit.Email;
                logUser.Age = edit.Age;
                logUser.Job = edit.Job;

                var result = await _userManager.UpdateAsync(logUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profil","Account");
                }
                else
                {
                    ModelState.AddModelError("", "Gagal menyimpan perubahan profil");
                    return View(edit);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout(string returnUrl)
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return Redirect("/home");
        }
    }
}
