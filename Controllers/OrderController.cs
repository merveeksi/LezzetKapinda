using System.Security.Claims;
using System.Text.Json;
using LezzetKapinda.Data;
using LezzetKapinda.Enums;
using LezzetKapinda.Models;
using LezzetKapinda.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LezzetKapinda.Controllers;

public sealed class OrderController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<OrderController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ISession _session => _httpContextAccessor.HttpContext?.Session;

    public OrderController(AppDbContext context,
    ILogger<OrderController> logger,
    IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

     // GET: /Order/Card
        [HttpGet]
        public async Task<IActionResult> Card()
        {
            try
            {
                // "Cart" anahtarıyla session'dan sepeti çekiyoruz.
                var cartJson = _session.GetString("Cart");
                List<OrderItem> cart = new();

                if (!string.IsNullOrEmpty(cartJson))
                {
                    cart = JsonSerializer.Deserialize<List<OrderItem>>(cartJson) ?? new List<OrderItem>();
                }


                var totalAmount = cart.Sum(item => item.Price * item.Quantity);

                // ViewModel oluştur
                var model = new OrderViewModel
                {
                    OrderItems = cart,
                    Addresses = await _context.Addresses
                        .AsNoTracking()
                        .Where(a => a.UserId == long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                        .ToListAsync(),
                    Address = await _context.Addresses
                        .FirstOrDefault(a => a.UserId == long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) && a.IsDefault)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet bilgileri getirilirken hata oluştu.");
                return View("Error"); // Hata sayfasına yönlendirebilirsiniz.
            }
        }

            // Start of Selection
            // POST: /Order/PlaceOrder
            [HttpPost]
            public async Task<IActionResult> PlaceOrder(OrderViewModel orderViewModel)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        // Model valid değilse, sepet sayfasına geri dönüyoruz.
                        return View("Card", orderViewModel);
                    }

                    // Sepeti session'dan alıyoruz
                    var cartJson = _session.GetString("Card");
                    List<OrderItem> cart = new();

                if (!string.IsNullOrEmpty(cartJson))
                {
                    cart = JsonSerializer.Deserialize<List<OrderItem>>(cartJson) ?? new List<OrderItem>();
                }
                    var card = string.IsNullOrEmpty(cartJson)
                        ? new List<OrderItem>()
                        : JsonSerializer.Deserialize<List<OrderItem>>(cartJson) ?? new List<OrderItem>();

                    // Kullanıcı bilgilerini al
                    var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!long.TryParse(userIdString, out long userId))
                    {
                        _logger.LogError("Kullanıcı ID'si alınamadı.");
                        return View("Error");
                    }

                    var userName = User.Identity?.Name;
                    if (string.IsNullOrEmpty(userName))
                    {
                        _logger.LogError("Kullanıcı adı alınamadı.");
                        return View("Error");
                    }

                    // Adresi al
                    var selectedAddressId = orderViewModel.AddressId;
                    var address = await _context.Addresses
                        .FirstOrDefaultAsync(a => a.Id == selectedAddressId && a.UserId == userId);

                    if (address == null)
                    {
                        _logger.LogError("Adres bulunamadı veya kullanıcıya ait değil.");
                        return View("Error");
                    }

                    // Yeni bir sipariş nesnesi oluştur
                    var newOrder = new Order
                    {
                        UserId = userId,       
                        UserName = userName,
                        Address = address,
                        OrderItems = cart,
                        TotalAmount = cart.Sum(item => item.Price * item.Quantity),
                        OrderDate = DateTimeOffset.Now,
                        Status = Status.Preparing
                    };
                    // Veritabanına kaydet
                    await _context.Orders.AddAsync(newOrder);
                    await _context.SaveChangesAsync();

                    // Sepeti temizle
                    _session.Remove("Cart");

                    // Sipariş onay sayfasına yönlendir
                    return RedirectToAction("OrderSuccess", new { id = newOrder.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Sipariş oluşturulurken bir hata oluştu.");
                    return View("Error");
                }
            }

        // GET: /Order/OrderSuccess/5
        [HttpGet]
        public async Task<IActionResult> OrderSuccess(long id)
        {
            try
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (order == null)
                {
                    return NotFound();
                }

                var model = new OrderViewModel
                {
                    Orders = new List<Order> { order },
                    OrderItems = order.OrderItems,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş onay sayfası görüntülenirken hata oluştu.");
                return View("Error");
            }
        }

        // GET (veya POST) /Order/AddToCart?productId=xyz
        [HttpPost]
        public async Task<IActionResult> AddToCart(long productId)
        {
            try
            {
                // 1. Session'dan mevcut sepeti çek
                var cartJson = _session.GetString("Cart");
                var cart = string.IsNullOrEmpty(cartJson)
            ? new List<OrderItem>()
            : JsonSerializer.Deserialize<List<OrderItem>>(cartJson) ?? new List<OrderItem>();

        // 2. Ürünü veritabanından çekin (örnek - kendi repository veya EF sorgunuz)
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            // Ürün bulunamadıysa hata veya başka bir işlem
            return NotFound("Ürün bulunamadı");
        }

        // 3. Sepette bu productId var mı kontrol et
        var existingItem = cart.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            // Adedi 1 artır
            // (OrderItem'daki Quantity init-only tanımlı, bu nedenle record veya modelinizi güncellenebilir şekilde düzenlemeniz gerekebilir.)
            // Şimdilik basit bir workaround: item'ı kaldırıp, quantity + 1 ile yeniden ekleyebilirsiniz.
            cart.Remove(existingItem);

            var updatedItem = existingItem with
            {
                Quantity = existingItem.Quantity + 1
            };
            cart.Add(updatedItem);
        }
        else
        {
            // 4. Yoksa yeni bir OrderItem oluştur
            var newItem = new OrderItem
            {
                Id = 0, // DB'ye kaydedeceğimizde otomatik artacak
                ProductId = product.Id,
                ProductName = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = 1,
                Total = product.Price * 1 // Şimdilik anlık total
            };
            cart.Add(newItem);
        }

        // 5. Güncel sepeti tekrar session'a yaz
        cartJson = JsonSerializer.Serialize(cart);
        _session.SetString("Cart", cartJson);

        // 6. Kullanıcıyı tekrar menüye veya sepet sayfasına yönlendirin
        return Json(new { success = true });
        // veya: return RedirectToAction("Card"); -> Sepet sayfası
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Ürün sepete eklenirken hata oluştu.");
        return View("Error");
    }
}

// POST: /Order/UpdateQuantity
[HttpPost]
public async Task<IActionResult> UpdateQuantity(long itemId, int change)
{
    try
    {
        var cartJson = _session.GetString("Cart");
        if (string.IsNullOrEmpty(cartJson))
        {
            return Json(new { success = false });
        }

        var cart = JsonSerializer.Deserialize<List<OrderItem>>(cartJson);
        var item = cart.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            return Json(new { success = false });
        }

        // Yeni miktar hesapla
        var newQuantity = item.Quantity + change;
        if (newQuantity < 1)
        {
            // Eğer miktar 1'den küçükse ürünü sepetten kaldır
            cart.Remove(item);
        }
        else
        {
            // Ürünü güncelle
            cart.Remove(item);
            var updatedItem = item with { Quantity = newQuantity };
            cart.Add(updatedItem);
        }

        // Sepeti güncelle
        _session.SetString("Cart", JsonSerializer.Serialize(cart));
        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Ürün miktarı güncellenirken hata oluştu.");
        return Json(new { success = false });
    }
}

// POST: /Order/RemoveItem
[HttpPost]
public async Task<IActionResult> RemoveItem(long itemId)
{
    try
    {
        var cartJson = _session.GetString("Cart");
        if (string.IsNullOrEmpty(cartJson))
        {
            return Json(new { success = false });
        }

        var cart = JsonSerializer.Deserialize<List<OrderItem>>(cartJson);
        var item = cart.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            return Json(new { success = false });
        }

        // Ürünü sepetten kaldır
        cart.Remove(item);

        // Sepeti güncelle
        _session.SetString("Cart", JsonSerializer.Serialize(cart));
        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Ürün sepetten kaldırılırken hata oluştu.");
        return Json(new { success = false });
    }
}

 [HttpPost]
        public IActionResult Checkout()
        {
            // Eğer kullanıcı login değilse => Account/Login yönlendir
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Aksi halde sipariş tamamlama aşaması
            // Kullanıcının kayıtlı adreslerinden birini seçmesini sağla
            // Siparişi veritabanına kaydet
            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            // Sipariş onay sayfası
            return View();
        }
}
