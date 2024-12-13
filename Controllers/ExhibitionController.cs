using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using WebApplication3.Models;
using Microsoft.EntityFrameworkCore;

public class ExhibitionController : Controller
{
    private readonly GalleryContext _context;

    public ExhibitionController(GalleryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Exhibition exhibition)
    {
        if (ModelState.IsValid)
        {
            _context.Exhibitions.Add(exhibition);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(exhibition);
    }

    [HttpPost]
    public async Task<IActionResult> AddArtworkToExhibition(int exhibitionId, int artworkId)
    {
        var exhibition = await _context.Exhibitions
            .Include(e => e.Artworks)
            .FirstOrDefaultAsync(e => e.Id == exhibitionId);

        if (exhibition == null)
        {
            return NotFound();
        }

        var artwork = await _context.Artworks.FindAsync(artworkId);
        if (artwork == null)
        {
            return NotFound();
        }

        exhibition.Artworks.Add(artwork);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { id = exhibitionId });
    }

    public async Task<IActionResult> Details(int id)
    {
        var exhibition = await _context.Exhibitions
            .Include(e => e.Artworks)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (exhibition == null)
        {
            return NotFound();
        }

        return View(exhibition);
    }
}
