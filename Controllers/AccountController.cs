using System;
using System.Security.Claims;
using LezzetKapinda.Data;
using LezzetKapinda.Models;
using LezzetKapinda.ValueObjects;
using LezzetKapinda.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LezzetKapinda.Controllers;

public sealed class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

     [HttpGet]
    public async Task<JsonResult> GetUserImage(UserName username)
    {
        // Eğer boş bir kullanıcı adı gönderilmişse varsayılan logoyu döndürün
        if (string.IsNullOrWhiteSpace(username.Value))
        {
            return Json(new { imageUrl = "/img/logo.png" });
        }

        // Kullanıcı adını küçük harfe çevirip karşılaştırmak, büyük/küçük harf duyarlılığını önler.
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName.Value.ToLower() == username.Value.ToLower());

        // Eğer kullanıcı bulunduysa ve profil resmi tanımlı ise
        if (user != null && !string.IsNullOrEmpty(user.ImageUrl))
        {
            return Json(new { imageUrl = user.ImageUrl });
        }

        // Kullanıcı bulunamazsa veya resmi tanımlı değilse varsayılan logo döndürün
        return Json(new { imageUrl = "/img/logo.png" });
    }

            
            // GET: /Account/Login
            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);


            // Veritabanından kullanıcıyı çekiyoruz
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName.Value == model.UserName.Value);

        // Kullanıcı var mı ve şifre doğru mu?
        if (user != null && user.VerifyPassword(model.Password))
        {
            // Claims oluşturuyoruz
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName.Value),
                new Claim(ClaimTypes.Role, user.Role) // Rolleri de ekleyelim
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme
            );

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe  // "Beni Hatırla"
            };

            // SignIn işlemi
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            // Giriş başarılı -> Anasayfa
            return RedirectToAction("Index", "Home");
        }

        // Eğer kullanıcı bulunamaz veya şifre yanlışsa:
        ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
        return View(model);
            }


            // GET: /Account/Logout
            [HttpGet]
            public async Task<IActionResult> Logout()
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("LogoutConfirmation");
            }

            // GET: /Account/LogoutConfirmation
            [HttpGet]
            public IActionResult LogoutConfirmation()
            {
                return View();
            }

        // GET: /Account/Register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Hataları loglayın.
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                return View(model);
            }

            // Kullanıcının veritabanında var mı diye kontrol
        var userExists = await _context.Users
            .AnyAsync(u => u.UserName.Value == model.UserName);

        if (userExists)
        {
            ModelState.AddModelError("UserName", "Bu kullanıcı adı zaten alınmış.");
            return View(model);
        }

        // Yeni user oluştur
        var user = new User
        {
            FullName = new FullName(model.FirstName, model.LastName),
            UserName = new UserName(model.UserName),
            Email = new Email(model.Email),
            Role = model.Role,
            CreatedOn = DateTimeOffset.UtcNow,
        };

        // Şifre hashleme
        user.SetPassword(model.Password);

        await _context.Users.AddAsync(user);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            // Kayıt başarılıysa Login sayfasına yönlendir
            return RedirectToAction("Login", "Account");
        }
        
            // Kayıt başarısızsa hata mesajı göster
            ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu.");
            return View(model);
        
    }
    
}
