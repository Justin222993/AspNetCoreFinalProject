using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class BuyerController : Controller
    {
        private readonly GalleryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BuyerController(GalleryContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null) return NotFound();

            var cart = await _context.Orders
                .Include(o => o.Artworks)
                .FirstOrDefaultAsync(o => o.UserId == user.Id && o.IsCart);

            if (cart == null)
            {
                cart = new Order
                {
                    UserId = user.Id,
                    IsCart = true
                };
                _context.Orders.Add(cart);
            }

            if (!cart.Artworks.Contains(artwork))
            {
                cart.Artworks.Add(artwork);
                cart.TotalPrice += artwork.Price;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Cart()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var cart = await _context.Orders
                .Include(o => o.Artworks)
                .FirstOrDefaultAsync(o => o.UserId == user.Id && o.IsCart);

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string accountNumber, string accountHolder)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var cart = await _context.Orders
                .Include(o => o.Artworks)
                .FirstOrDefaultAsync(o => o.UserId == user.Id && o.IsCart);

            if (cart == null || !cart.Artworks.Any())
                return BadRequest("Your cart is empty.");

            if (string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(accountHolder))
            {
                return BadRequest("Invalid bank account details.");
            }

            cart.IsCart = false;
            cart.OrderDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("MyPurchases");
        }


        public async Task<IActionResult> MyPurchases()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var purchases = await _context.Orders
                .Where(o => o.UserId == user.Id && !o.IsCart)
                .Include(o => o.Artworks)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(purchases);
        }

    }
}
