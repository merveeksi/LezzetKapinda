using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LezzetKapinda.Models;
using LezzetKapinda.ViewModels;
using LezzetKapinda.Data;
using Microsoft.EntityFrameworkCore;

namespace LezzetKapinda.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context
        .Categories
        .AsNoTracking()
        .ToListAsync();

        var products = await _context
        .Products
        .Include(p => p.Category)
        .Where(p => p.IsActive)
        .AsNoTracking()
        .ToListAsync();
        
        var categoriesDictionary = categories.ToDictionary(c => c.Id,
        c => products.Where(p => p.CategoryId == c.Id).ToList());

         var viewModel = new HomeViewModel
        {
            FullName = new ValueObjects.FullName("Merve", "Ekşi"),
            Categories = categories,
            Products = products,
            Counters = await _context.Counters.AsNoTracking().ToListAsync(),
            Testimonials = await _context.Testimonials.Take(3).AsNoTracking().ToListAsync(),
            Users = await _context.Users.AsNoTracking().ToListAsync(),
            UserOrderImages = await _context.UserOrderImages.AsNoTracking().ToListAsync()
        };
        
        return View(viewModel);
    }


    public async Task<IActionResult> Menu()
        {
            try
            {
                var viewModel = new HomeViewModel
            {
                Categories = await _context.Categories.AsNoTracking().ToListAsync(),
                Products = await _context.Products.AsNoTracking().ToListAsync(),
            };
                return View(viewModel); // Views/Home/Menu.cshtml
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Home/Menu verileri alınırken bir hata oluştu");
                return View("Error");
            }
        }

        // Testimonial
        public async Task<IActionResult> Testimonial()
        {
            var model = new HomeViewModel
            {
                Testimonials = await _context.Testimonials.AsNoTracking().ToListAsync(),
                Users = await _context.Users.AsNoTracking().ToListAsync()
            };
            return View(model); // Views/Home/Testimonial.cshtml
        }

        // WhyUs
        public async Task<IActionResult> WhyUs()
        {
            var model = new HomeViewModel
            {
                // Bu sayfa hangi verileri kullanacaksa doldurun
            };
            return View(model); // Views/Home/WhyUs.cshtml
        }

        // About
        public async Task<IActionResult> About()
        {
            var viewModel = new HomeViewModel
            {
                // About sayfasında ihtiyaç duyulan verileri doldurun
            };
            return View(viewModel); // Views/Home/About.cshtml
        }

        // Contact
        
        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactViewModel model)
        {
            try
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                message = "Mesaj gönderilirken hata oluştu. Lütfen daha sonra tekrar deneyiniz."
            });
        }

        var contactMessage = new ContactMessage
        {
            Name = model.Name,
            Email = model.Email,
            Subject = model.Subject,
            Message = model.Message,
            SentDate = DateTimeOffset.UtcNow
        };

        await _context.ContactMessages.AddAsync(contactMessage);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Mesajınız başarıyla gönderildi! En kısa sürede sizinle iletişime geçeceğiz." });
        ModelState.Clear();
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz."});
        ModelState.Clear();
    }
        }

        // Gallery
        public async Task<IActionResult> GalleryAsync()
        {
            var viewModel = new GalleryViewModel
            {
                UserOrderImages = await _context.UserOrderImages.AsNoTracking().ToListAsync()
            };
            return View(viewModel); // Views/Home/Gallery.cshtml
        }

        public async Task<IActionResult> ErrorAsync(int statusCode)
        {
            if (statusCode == 500)
            {
                Response.StatusCode = 500;
                return View("Error500");
            }
            else if (statusCode == 400)
            {
                Response.StatusCode = 400;
                return View("Error400");
            }
            else
            {
                Response.StatusCode = statusCode;
                return View("Error");
            }
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}