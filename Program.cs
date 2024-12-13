using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GalleryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<GalleryContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("Artist"))
        await roleManager.CreateAsync(new IdentityRole("Artist"));
    if (!await roleManager.RoleExistsAsync("Buyer"))
        await roleManager.CreateAsync(new IdentityRole("Buyer"));

    var context = scope.ServiceProvider.GetRequiredService<GalleryContext>();

    if (!context.Artists.Any())
    {
        var artist1 = new Artist
        {
            FullName = "John Doe",
            Bio = "A passionate artist who loves to create abstract and modern art.",
            ProfileImageUrl = "/images/artist1.jpg",
            UserId = "default-user-id-1"
        };

        var artist2 = new Artist
        {
            FullName = "Jane Smith",
            Bio = "A contemporary artist who focuses on realism and portraits.",
            ProfileImageUrl = "/images/artist2.jpg",
            UserId = "default-user-id-2"
        };

        context.Artists.AddRange(artist1, artist2);
        await context.SaveChangesAsync();
    }

    if (!context.Exhibitions.Any())
    {
        var exhibition1 = new Exhibition
        {
            Title = "Summer Art Exhibition",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(1),
            FloorPlanUrl = "/images/exhibition1.jpg",
            Artworks = new List<Artwork>()
        };

        var exhibition2 = new Exhibition
        {
            Title = "Modern Art Showcase",
            StartDate = DateTime.Now.AddMonths(2),
            EndDate = DateTime.Now.AddMonths(3),
            FloorPlanUrl = "/images/exhibition2.jpg",
            Artworks = new List<Artwork>()
        };

        context.Exhibitions.AddRange(exhibition1, exhibition2);
        await context.SaveChangesAsync();
    }

    if (!context.Artworks.Any())
    {
        var artist1 = context.Artists.FirstOrDefault(a => a.FullName == "John Doe");
        var artist2 = context.Artists.FirstOrDefault(a => a.FullName == "Jane Smith");

        var exhibition1 = context.Exhibitions.FirstOrDefault(e => e.Title == "Summer Art Exhibition");
        var exhibition2 = context.Exhibitions.FirstOrDefault(e => e.Title == "Modern Art Showcase");

        var artworks = new List<Artwork>
    {
        new Artwork
        {
            Title = "Sunset Serenity",
            Medium = "Oil on Canvas",
            Size = "24x36 inches",
            Price = 600,
            Description = "A peaceful sunset over a calm sea.",
            ImageUrl = "/images/artwork1.jpg",
            Artist = artist1
        },
        new Artwork
        {
            Title = "Modern Abstract",
            Medium = "Acrylic on Canvas",
            Size = "30x40 inches",
            Price = 800,
            Description = "A bold and colorful abstract painting.",
            ImageUrl = "/images/artwork2.jpg",
            Artist = artist2
        },
        new Artwork
        {
            Title = "Forest Dream",
            Medium = "Watercolor on Paper",
            Size = "18x24 inches",
            Price = 450,
            Description = "A dreamy depiction of a lush forest.",
            ImageUrl = "/images/artwork3.jpg",
            Artist = artist1
        },
        new Artwork
        {
            Title = "City Lights",
            Medium = "Digital Art",
            Size = "1920x1080 pixels",
            Price = 300,
            Description = "A digital artwork capturing the beauty of city lights.",
            ImageUrl = "/images/artwork4.jpg",
            Artist = artist2
        },
        new Artwork
        {
            Title = "Ocean Waves",
            Medium = "Oil on Canvas",
            Size = "24x36 inches",
            Price = 700,
            Description = "Dynamic waves crashing onto the shore.",
            ImageUrl = "/images/artwork5.jpg",
            Artist = artist1
        },
        new Artwork
        {
            Title = "Golden Fields",
            Medium = "Acrylic on Canvas",
            Size = "24x36 inches",
            Price = 650,
            Description = "A tranquil field bathed in golden sunlight.",
            ImageUrl = "/images/artwork6.jpg",
            Artist = artist2
        },
        new Artwork
        {
            Title = "Starry Night",
            Medium = "Oil on Canvas",
            Size = "36x48 inches",
            Price = 900,
            Description = "A night sky full of stars over a quiet village.",
            ImageUrl = "/images/artwork7.jpg",
            Artist = artist1
        },
        new Artwork
        {
            Title = "Mountain Majesty",
            Medium = "Watercolor on Paper",
            Size = "18x24 inches",
            Price = 500,
            Description = "Towering mountains against a clear blue sky.",
            ImageUrl = "/images/artwork8.jpg",
            Artist = artist2
        },
        new Artwork
        {
            Title = "Autumn Path",
            Medium = "Oil on Canvas",
            Size = "24x36 inches",
            Price = 600,
            Description = "A winding path through an autumn forest.",
            ImageUrl = "/images/artwork9.jpg",
            Artist = artist1
        }
    };

        context.Artworks.AddRange(artworks);

        await context.SaveChangesAsync();

        exhibition1.Artworks.Add(context.Artworks.FirstOrDefault(a => a.Title == "Sunset Serenity"));
        exhibition1.Artworks.Add(context.Artworks.FirstOrDefault(a => a.Title == "Forest Dream"));
        exhibition1.Artworks.Add(context.Artworks.FirstOrDefault(a => a.Title == "Ocean Waves"));
        exhibition1.Artworks.Add(context.Artworks.FirstOrDefault(a => a.Title == "Starry Night"));
        exhibition2.Artworks.Add(context.Artworks.FirstOrDefault(a => a.Title == "Modern Abstract"));
        exhibition2.Artworks.Add(context.Artworks.FirstOrDefault(a => a.Title == "City Lights"));
        exhibition2.Artworks.Add(context.Artworks.FirstOrDefault(a => a.Title == "Golden Fields"));
        exhibition2.Artworks.Add(context.Artworks.FirstOrDefault(a => a.Title == "Mountain Majesty"));

        await context.SaveChangesAsync();
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
