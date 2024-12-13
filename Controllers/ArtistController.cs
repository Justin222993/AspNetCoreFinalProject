using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Artist")]
    public class ArtistController : Controller
    {
        private readonly GalleryContext _context;

        public ArtistController(GalleryContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var artist = await _context.Artists
                .Include(a => a.Artworks)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (artist == null)
            {
                return NotFound("Artist not found for the current user.");
            }

            var model = new ArtistProfileViewModel
            {
                FullName = artist.FullName,
                Bio = artist.Bio,
                ProfileImageUrl = artist.ProfileImageUrl,
                Artworks = artist.Artworks.Select(a => new ArtworkViewModel
                {
                    Title = a.Title,
                    Description = a.Description,
                    ImageUrl = a.ImageUrl
                }).ToList()
            };

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var artist = await _context.Artists
                .Include(a => a.Artworks)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        [HttpGet]
        public IActionResult AddArtworkForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddArtwork(Artwork model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                    }
                }

                return View("AddArtworkForm", model);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.UserId == userId);

            if (artist == null)
            {
                return NotFound("Artist not found for the current user.");
            }

            model.ArtistId = artist.Id;

            _context.Artworks.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }

        [HttpGet]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> CreateExhibition()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var artist = await _context.Artists
                .Include(a => a.Artworks)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (artist == null)
            {
                return NotFound("Artist not found.");
            }

            var exhibition = new Exhibition
            {
                Artworks = artist.Artworks.ToList()
            };

            return View(exhibition);
        }


        [HttpPost]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> CreateExhibition(Exhibition exhibition)
        {
            if (!ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var artist = await _context.Artists
                    .Include(a => a.Artworks)
                    .FirstOrDefaultAsync(a => a.UserId == userId);

                if (artist != null)
                {
                    exhibition.Artworks = artist.Artworks.ToList();
                }

                return View(exhibition);
            }

            var selectedArtworkIds = Request.Form["Artworks"]
                .Select(id => int.Parse(id))
                .ToList();

            var selectedArtworks = await _context.Artworks
                .Where(a => selectedArtworkIds.Contains(a.Id))
                .ToListAsync();

            exhibition.Artworks = selectedArtworks;

            _context.Exhibitions.Add(exhibition);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Exhibition", new { id = exhibition.Id });
        }





    }
}
