using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly GalleryContext _context;

        public HomeController(GalleryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var artworks = await _context.Artworks.Include(a => a.Artist).ToListAsync();
            var artists = await _context.Artists.ToListAsync();
            var exhibitions = await _context.Exhibitions.ToListAsync();

            var model = new HomeViewModel
            {
                Artworks = artworks,
                Artists = artists,
                Exhibitions = exhibitions
            };

            return View(model);
        }
    }
}
