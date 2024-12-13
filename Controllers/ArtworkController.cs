using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Controllers
{
    public class ArtworkController : Controller
    {
        private readonly GalleryContext _context;

        public ArtworkController(GalleryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            var artwork = await _context.Artworks
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artwork == null)
                return NotFound();

            return View(artwork);
        }
    }

}
